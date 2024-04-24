using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.Services.Interfaces;
using SmartShopMobileApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.ViewModels
{
    public partial class LogInViewModel: ObservableObject
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        private string _email;
        [ObservableProperty]
        private string _password;
        [ObservableProperty]
        private UserDTO _existingUser;
        public LogInViewModel(IAuthService authService)
        {
            _manageData = new ManageData();
            _authService = authService;
        }
        private IManageData _manageData;

        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
        }

        [RelayCommand]
        private async void LogIn()
        {
            _manageData.SetStrategy(new GetData());
            ExistingUser = null;
            ExistingUser = await _manageData.GetDataAndDeserializeIt<UserDTO>($"User/GetUserByEmailAndPassword?email={Email}&password={HashPassword(Password)}", "");
           
            //AuthenticationResultHelper.ActiveUser.UserId = ExistingUser.UserID;
            //AuthenticationResultHelper.ActiveUser.FirstName = ExistingUser.FirstName;
            //AuthenticationResultHelper.ActiveUser.LastName = ExistingUser.LastName;
            //AuthenticationResultHelper.ActiveUser.Email = ExistingUser.Email;
            //AuthenticationResultHelper.ActiveUser.PreferredLanguage = ExistingUser.PreferredLanguage;
            //AuthenticationResultHelper.ActiveUser.PreferredCurrency = ExistingUser.PreferredCurrency;


            var error = await _authService.LoginAsync(new LoginRequestDTO(ExistingUser.Email, ExistingUser.Password));
            if (string.IsNullOrWhiteSpace(error))
            {
                await Shell.Current.GoToAsync("//AppView");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", error, "Ok");
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

        [RelayCommand]
        private async Task OpenSignUpPage()
        {
            await Shell.Current.GoToAsync("//SignUpView");
        }
    }
}
