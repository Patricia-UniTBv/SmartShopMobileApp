using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.Services;
using SmartShopMobileApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ExistingUser = await _manageData.GetDataAndDeserializeIt<UserDTO>($"User/GetUserByEmailAndPassword?email={Email}&password={Password}", "");

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
    }
}
