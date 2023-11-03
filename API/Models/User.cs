using System;
using System.Collections.Generic;

namespace API.Models;

public partial class User
{
    public int UserID { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? Birthdate { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
}
