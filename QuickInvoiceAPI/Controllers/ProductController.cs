using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickInvoiceAPI.Models;
using Microsoft.AspNetCore.Cors;
using QuickInvoiceAPI.DTOs;

namespace QuickInvoiceAPI.Controllers
{
    [EnableCors("RulesCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly QuickInvoiceBdContext _bdContext;

        public ProductController(QuickInvoiceBdContext context)
        {
            _bdContext = context;
        }

        [HttpGet]
        //[Route("List")]
        public IActionResult List()
        {
            try
            {
                List<ProductDTO> products = _bdContext.Products.Select(product => ProductToDTO(product)).ToList();

                return Ok(products);
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpGet("{code}")]
        //[Route("Get/{codeProduct}")]
        public IActionResult Get(string code)
        {
            try
            {
                Product? product = _bdContext.Products.Find(code);

                if (product == null) return NotFound();

                return Ok(ProductToDTO(product));
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpPost]
        //[Route("Add")]
        public IActionResult Add([FromBody] ProductDTO productDTO)
        {
            try
            {
                Product product = new Product()
                {
                    Code = productDTO.Code,
                    Description = productDTO.Description,
                    Price = productDTO.Price,
                    ApplyIva = productDTO.ApplyIva
                };

                _bdContext.Products.Add(product);
                _bdContext.SaveChanges();

                return CreatedAtAction(
                    nameof(Get),
                    new { code = product.Code },
                    ProductToDTO(product));
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpPut("{code}")]
        //[Route("Edit")]
        public IActionResult Edit(string code, [FromBody] ProductDTO productDTO)
        {
            if (code != productDTO.Code) return BadRequest();

            try
            {
                Product? product = _bdContext.Products.Find(productDTO.Code);

                if (product == null) return NotFound();

                product.Description = productDTO.Description is null ? product.Description : productDTO.Description;
                product.Price = productDTO.Price <= 0 ? product.Price : productDTO.Price;
                product.ApplyIva = productDTO.ApplyIva is null ? product.ApplyIva : productDTO.ApplyIva;
                product.Active = productDTO.Active is null ? product.Active : productDTO.Active;

                _bdContext.SaveChanges();

                return NoContent();
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpDelete("{code}")]
        //[Route("Delete/{codeProduct}")]
        public IActionResult Delete(string code)
        {
            Product? product = _bdContext.Products.Find(code);

            if (product == null) return NotFound();

            try
            {
                _bdContext.Products.Remove(product);
                _bdContext.SaveChanges();

                return NoContent();
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        private static ProductDTO ProductToDTO(Product product) =>
           new ProductDTO
           {
               Code = product.Code,
               Description = product.Description,
               Price = product.Price,
               ApplyIva = product.ApplyIva,
               Active = product.Active
           };

    }
}
