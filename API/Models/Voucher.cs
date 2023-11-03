using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Voucher
{
    public int VoucherID { get; set; }

    public int UserID { get; set; }

    public int SupermarketID { get; set; }

    public string? CardNumber { get; set; }

    public int? EarnedPoints { get; set; }

    public virtual Supermarket Supermarket { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
