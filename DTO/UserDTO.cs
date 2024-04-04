using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UserDTO
    {
        public int UserID { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateTime? Birthdate { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;
        public string ImageUrl => "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460__340.png";

        public string? PreferredLanguage { get; set; }
    }

}
