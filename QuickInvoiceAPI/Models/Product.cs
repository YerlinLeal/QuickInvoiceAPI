using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QuickInvoiceAPI.Models;

public partial class Product
{
    public string Code { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public bool? ApplyIva { get; set; }

}
