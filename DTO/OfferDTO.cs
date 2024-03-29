using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class OfferDTO
    {
        public int OfferId { get; set; }

        public double OfferPercentage { get; set; }

        public DateTime? OfferStartDate { get; set; }

        public DateTime? OfferEndDate { get; set; }

        public int SupermarketId { get; set; }

        public int ProductId { get; set; }
        public decimal OldPrice { get; set; }

        public decimal NewPrice { get; set; }

        public virtual ProductDTO Product { get; set; } = null!;
        public virtual SupermarketDTO Supermarket { get; set; } = null!;

    }
}
