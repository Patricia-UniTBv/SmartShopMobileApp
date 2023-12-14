using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Transaction
{
    public int TransactionID { get; set; }

    public int ShoppingCartID { get; set; }

    public DateTime TransactionDate { get; set; }

    public double TotalAmount { get; set; }

    public string? Barcode { get; set; }

    public virtual ShoppingCart ShoppingCart { get; set; } = null!;
}
