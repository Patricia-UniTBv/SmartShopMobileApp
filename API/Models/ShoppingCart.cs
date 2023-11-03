using System;
using System.Collections.Generic;

namespace API.Models;

public partial class ShoppingCart
{
    public int ShoppingCartID { get; set; }

    public int UserID { get; set; }

    public DateTime CreationDate { get; set; }

    public double TotalAmount { get; set; }

    public bool? IsTransacted { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User User { get; set; } = null!;
}
