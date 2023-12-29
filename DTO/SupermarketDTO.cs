using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class SupermarketDTO
    {
        public int SupermarketID { get; set; }

        public string Name { get; set; } = null!;

        public string ImageUrl
        {
            get
            {
                return "shopping_cart.png";
            }
        }
    }
}
