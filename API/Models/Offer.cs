using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Offer
{
    public int OfferId { get; set; }

    public double OfferPercentage { get; set; }

    public DateTime? OfferStartDate { get; set; }

    public DateTime? OfferEndDate { get; set; }

    public int SupermarketId { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Supermarket Supermarket { get; set; } = null!;
}
