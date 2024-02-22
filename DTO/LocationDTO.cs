using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class LocationDTO
    {
        public int LocationID { get; set; }

        public int SupermarketID { get; set; }

        public string? Address { get; set; }

        public double? Latitude { get; set; }

        public double? Longidute { get; set; }

        public virtual SupermarketDTO Supermarket { get; set; } = null!;
    }
}
