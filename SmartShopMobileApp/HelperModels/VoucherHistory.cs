using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.HelperModels
{
    public class VoucherHistory
    {
        public DateTime CartCreationDate { get; set; }

        public double TotalAmount { get; set; }

        public bool? IsTransacted { get; set; }

        public string ValueModification { get; set; }

        public string ValueModificationTextColor { get; set; }
    }
}
