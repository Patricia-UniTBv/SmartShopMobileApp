using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        public string Name { get; set; } = null!;

        public double Price { get; set; }

        public string? Category { get; set; }

        public string? Barcode { get; set; }
    }
}
