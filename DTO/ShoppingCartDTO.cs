using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DTO
{

    public partial class ShoppingCartDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShoppingCartID { get; set; }

        public int UserID { get; set; }

        public DateTime CreationDate { get; set; }

        public double TotalAmount { get; set; }

        public bool? IsTransacted { get; set; }

        public virtual UserDTO User { get; set; } = null!;
    }

}
