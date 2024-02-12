using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public partial class CartItemDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartItemID { get; set; }

        public int ShoppingCartID { get; set; }

        public int ProductID { get; set; }

        public double? Quantity { get; set; }

        public virtual ProductDTO Product { get; set; } = null!;

    }

}
