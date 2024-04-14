using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ProductDTO
    {
        public int ProductID { get; set; }

        public int CategoryID { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public string Barcode { get; set; } = null!;

        public string? Unit { get; set; }

        public double? Stock { get; set; }

        public int SupermarketID { get; set; }

        public byte[]? Image { get; set; }
        public string ImageUrl 
        {
            get
            {
               return "https://nayemdevs.com/wp-content/uploads/2020/03/default-product-image.png";
            }
        }
        public string? ImageSource { get; set; }
        public double? Quantity { get; set; }
        public string? Currency { get; set; }


    }
}
