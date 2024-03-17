using System;
using System.Collections.Generic;

namespace QuickInvoiceAPI.Models;

public partial class SaleProduct
{
    public int SaleId { get; set; }

    public string ProductCode { get; set; } = null!;

    public int? Quantity { get; set; }

    public decimal? Amount { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Sale Sale { get; set; } = null!;
}
