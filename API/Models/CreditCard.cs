using System;
using System.Collections.Generic;

namespace API.Models;

public partial class CreditCard
{
    public int CardID { get; set; }

    public string HolderName { get; set; } = null!;

    public string CardNumber { get; set; } = null!;

    public int YearOfExpiration { get; set; }

    public int MonthOfExpiration { get; set; }

    public string CVV { get; set; } = null!;

    public string CardIdentification { get; set; } = null!;

    public int UserID { get; set; }

    public virtual User User { get; set; } = null!;
}
