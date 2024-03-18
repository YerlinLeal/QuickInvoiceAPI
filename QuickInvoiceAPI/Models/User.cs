using System;
using System.Collections.Generic;

namespace QuickInvoiceAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool? Active { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
