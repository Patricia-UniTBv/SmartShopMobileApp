using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class TransactionDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionID { get; set; }

        public int ShoppingCartID { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string Barcode { get; set; } = null!;

        public decimal? VoucherDiscount { get; set; }

    }
}
