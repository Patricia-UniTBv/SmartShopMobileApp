using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Supermarket
{
    public int SupermarketID { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
}
