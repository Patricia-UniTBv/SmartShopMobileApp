using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Product
{
    public int ProductID { get; set; }

    public int CategoryID { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string Barcode { get; set; } = null!;

    public string? Unit { get; set; }

    public double? Stock { get; set; }

    public int SupermarketID { get; set; }

    public byte[]? Image { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category Category { get; set; } = null!;

    public virtual Supermarket Supermarket { get; set; } = null!;
}
