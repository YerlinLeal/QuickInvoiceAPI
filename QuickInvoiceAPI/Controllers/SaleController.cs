using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickInvoiceAPI.Models;
using Microsoft.AspNetCore.Cors;
using QuickInvoiceAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace QuickInvoiceAPI.Controllers
{
    [EnableCors("RulesCors")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SaleController : ControllerBase
    {
        public readonly QuickInvoiceBdContext _bdContext;

        public SaleController(QuickInvoiceBdContext context)
        {
            _bdContext = context;
        }

        [HttpGet]
        public IActionResult List()
        {
            try
            {
                List<SaleDTO> sales = _bdContext.Sales
                    .Include(sale => sale.User)
                    .Select(sale => SaleToDTO(sale)).ToList();

                return Ok(sales);
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                SaleDTO? sale = _bdContext.Sales
                    .Where(sale => sale.Id == id)
                    .Include(sale => sale.User)
                    .Include(sale => sale.SaleProducts)
                    .Select(sale => SaleToDTO(sale)).FirstOrDefault();

                if (sale == null) return NotFound();

                return Ok(sale);
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody] SaleDTO saleDTO)
        {
            try
            {
                Sale sale = new Sale()
                {
                    TotalAmount = saleDTO.TotalAmount,
                    UserId = saleDTO.UserId,
                    User = null,
                    SaleProducts = saleDTO.Products.Select(product => new SaleProduct()
                    {
                        ProductCode = product.Code,
                        Description = product.Description,
                        Price = product.Price,
                        Quantity = product.Quantity,
                        ApplyIva = product.ApplyIva
                    }).ToList()
                };

                _bdContext.Sales.Add(sale);
                _bdContext.SaveChanges();

                return CreatedAtAction(
                    nameof(Get),
                    new { id = sale.Id },
                    SaleToDTO(sale));
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        private static SaleDTO SaleToDTO(Sale sale) =>
           new SaleDTO
           {
               Id = sale.Id,
               TotalAmount = sale.TotalAmount,
               Date = sale.Date,
               UserId = sale.UserId,
               UserName = sale.User != null ? sale.User.UserName : String.Empty,
               Products = sale.SaleProducts != null ? sale.SaleProducts.Select(saleProduct => new SaleProductDTO()
               {
                   Code = saleProduct.ProductCode,
                   Price = saleProduct.Price,
                   Quantity = saleProduct.Quantity,
                   ApplyIva = saleProduct.ApplyIva,
                   Description = saleProduct.Description
               }).ToList() : new List<SaleProductDTO>()
           };
    }
}
