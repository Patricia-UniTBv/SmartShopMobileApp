﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.Models
{
    public class Invoice
    {
        private string description;
        private string price;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Price
        {
            get { return price; }
            set { price = value; }
        }
    }
}
