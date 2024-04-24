using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class LoggedInUser
    {
        public int UserId { get; }
        public string LastName { get; }
        public string FirstName { get; }
        public string Email { get; }
        public string Password { get; }
        public string PreferredLanguage { get; }
        public string PreferredCurrency { get; }

        public LoggedInUser(int id, string lastName, string firstName, string email, string password, string preferredLanguage, string preferredCurrency)
        {
            UserId = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            PreferredLanguage = preferredLanguage;
            PreferredCurrency = preferredCurrency;
        }
    }
}
