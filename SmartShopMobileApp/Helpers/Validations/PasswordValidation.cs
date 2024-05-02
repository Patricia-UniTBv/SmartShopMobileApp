using SmartShopMobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmartShopMobileApp.Helpers.Validations
{
    public class PasswordValidation : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var isValid = IsValidPassword(e.NewTextValue);
            ((Entry)sender).TextColor = isValid ? Colors.Black : Colors.Red;

            if (((Entry)sender).BindingContext is SignUpViewModel viewModel)
            {
                viewModel.IsPasswordValid = isValid;
            }
        }

        private bool IsValidPassword(string password)
        {
            return !string.IsNullOrEmpty(password) && password.Length >= 5 &&
                   Regex.IsMatch(password, "[A-Z]") && 
                   Regex.IsMatch(password, "[0-9]") && 
                   Regex.IsMatch(password, "[^a-zA-Z0-9]"); 
        }
    }
}
