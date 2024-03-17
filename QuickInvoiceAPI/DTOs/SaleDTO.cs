namespace QuickInvoiceAPI.DTOs
{
    public class SaleDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public decimal? TotalAmount { get; set; }
        public IEnumerable<SaleProductDTO> Products { get; set; } = new List<SaleProductDTO>();
    }
}