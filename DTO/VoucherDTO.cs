using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class VoucherDTO
    {
        public int VoucherID { get; set; }

        public int UserID { get; set; }

        public int SupermarketID { get; set; }

        public string? CardNumber { get; set; }

        public int? EarnedPoints { get; set; }

    }
}
