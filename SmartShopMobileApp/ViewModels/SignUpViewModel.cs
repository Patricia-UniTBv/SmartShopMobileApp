using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.ViewModels
{
    public partial class SignUpViewModel : ObservableObject
    {
        public SignUpViewModel()
        {
            _manageData = new ManageData();
        }

        private IManageData _manageData;
        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
        }

        [ObservableProperty]
        private string _firstName;

        [ObservableProperty]
        private string _lastName;

        [ObservableProperty]
        private string _emailAddress;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private DateTime birthDate;

        [RelayCommand]
        private async Task SignUp()
        {
            try
            {
                _manageData.SetStrategy(new CreateData());

                string hashedPassword = HashPassword(Password);

                await _manageData.GetDataAndDeserializeIt<UserDTO>($"User/AddNewUser?emailAddress={EmailAddress}&password={hashedPassword}&firstName={FirstName}&lastName={LastName}&birthdate={BirthDate}", "");

                await Shell.Current.GoToAsync("//LogInView");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static string HashPassword(string password)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

    }
}
