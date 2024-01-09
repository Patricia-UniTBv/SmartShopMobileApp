using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Transaction
{
    public int TransactionID { get; set; }

    public int ShoppingCartID { get; set; }

    public DateTime TransactionDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Barcode { get; set; }

    public decimal? VoucherDiscount { get; set; }

    public virtual ShoppingCart ShoppingCart { get; set; } = null!;
}
