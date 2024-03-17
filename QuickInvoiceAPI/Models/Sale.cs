using System;
using System.Collections.Generic;

namespace QuickInvoiceAPI.Models;

public partial class Sale
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime? Date { get; set; }

    public virtual ICollection<SaleProduct> SaleProducts { get; set; } = new List<SaleProduct>();

    public virtual User? User { get; set; } = new User();
}
