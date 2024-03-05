using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using Org.BouncyCastle.Security;
using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.ViewModels
{
    public partial class LogInViewModel:ObservableObject
    {
        public LogInViewModel()
        {
            _manageData = new ManageData();
        }

        private IManageData _manageData;
        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
        }

        public string Email { get; set; }
        public string Password { get; set; }

        [RelayCommand]
        private async Task LogIn()
        {
            try
            {
                _manageData.SetStrategy(new CreateData());

                //string hashedPassword = HashPassword(Password);

                AuthenticationResultHelper.ActiveUser = await _manageData.GetDataAndDeserializeIt<UserDTO>($"User/Login?email={Email}&password={Password}", "");

                Application.Current.MainPage =  new AppShell();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [RelayCommand]
        private Task OpenSignUpPage()
        {
            try
            {
                _ = Application.Current.MainPage.Navigation.PushAsync(new NavigationPage(new SignUpView()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return Task.CompletedTask;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
