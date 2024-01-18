using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Location
{
    public int LocationID { get; set; }

    public int SupermarketID { get; set; }

    public string? Address { get; set; }

    public double? Latitude { get; set; }

    public double? Longidute { get; set; }

    public virtual Supermarket Supermarket { get; set; } = null!;
}
