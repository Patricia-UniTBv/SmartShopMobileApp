using System;
using System.Collections.Generic;

namespace API.Models;

public partial class CartItem
{
    public int CartItemID { get; set; }

    public int ShoppingCartID { get; set; }

    public int ProductID { get; set; }

    public double? Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ShoppingCart ShoppingCart { get; set; } = null!;
}
