using System;
using System.Collections.Generic;

namespace QuickInvoiceAPI.Models;

public partial class SaleProduct
{
    public int SaleId { get; set; }

    public string ProductCode { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public bool? ApplyIva { get; set; }

    public virtual Sale Sale { get; set; } = null!;
}
