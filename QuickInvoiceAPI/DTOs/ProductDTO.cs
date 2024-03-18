namespace QuickInvoiceAPI.DTOs
{
    public class ProductDTO
    {
        public string Code { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public bool? ApplyIva { get; set; }
    }
}
