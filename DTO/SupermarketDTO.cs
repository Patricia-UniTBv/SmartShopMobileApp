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
                    return "https://upload.wikimedia.org/wikipedia/commons/thumb/9/91/Lidl-Logo.svg/2048px-Lidl-Logo.svg.png";
                   // return "lidl.png";
                }
                else if (Name == "Carrefour")
                {
                    return "https://upload.wikimedia.org/wikipedia/commons/thumb/5/5b/Carrefour_logo.svg/2554px-Carrefour_logo.svg.png";
                }
                else 
                {
                    return "https://upload.wikimedia.org/wikipedia/commons/thumb/4/44/Kaufland_201x_logo.svg/2048px-Kaufland_201x_logo.svg.png";
                }
            }
        }
    }
}
