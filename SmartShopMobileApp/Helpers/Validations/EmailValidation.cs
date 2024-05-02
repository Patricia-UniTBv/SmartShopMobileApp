using Microsoft.Maui.ApplicationModel.Communication;
using SmartShopMobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmartShopMobileApp.Helpers.Validations
{
    public class EmailValidation : Behavior<Entry>
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
            string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            bool isEmailValid = Regex.IsMatch(e.NewTextValue, emailPattern);
            ((Entry)sender).TextColor = isEmailValid ? Colors.Black : Colors.Red;

            if (((Entry)sender).BindingContext is SignUpViewModel viewModel)
            {
                viewModel.IsEmailValid = isEmailValid;
            }
        }
    }
}
