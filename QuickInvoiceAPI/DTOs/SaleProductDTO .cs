namespace QuickInvoiceAPI.DTOs
{
    public class SaleProductDTO
    {
        public string Code { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal? Price { get; set; }

        public bool? ApplyIva { get; set; }
        public int? Quantity { get; set; }

    }
}
