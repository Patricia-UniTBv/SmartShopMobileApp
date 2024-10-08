﻿using System;
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

        public decimal TotalAmount { get; set; }

        public bool? IsTransacted { get; set; }

        public int? SupermarketID { get; set; }

        public virtual UserDTO User { get; set; } = null!;
        public virtual SupermarketDTO? Supermarket { get; set; }
        public virtual ICollection<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();
        public List<ProductDTO> CartItemsAsProducts { get; set; } = new List<ProductDTO>();

    }

}
