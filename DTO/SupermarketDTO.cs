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
                if (Name == "Lidl")
                {
                    return "lidl.png";
                }
                else if (Name == "Carrefour")
                {
                    return "carrefour.png";
                }
                else 
                {
                    return "kaufland.png";
                }
            }
        }
    }
}
