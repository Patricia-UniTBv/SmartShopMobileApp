﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CreditCardDTO
    {
        public int CardID { get; set; }

        public string HolderName { get; set; } = null!;

        public string CardNumber { get; set; } = null!;

        public int YearOfExpiration { get; set; }

        public int MonthOfExpiration { get; set; }

        public string CVV { get; set; } = null!;

        public string CardIdentification { get; set; } = null!;

        public int UserID { get; set; }
    }
}
