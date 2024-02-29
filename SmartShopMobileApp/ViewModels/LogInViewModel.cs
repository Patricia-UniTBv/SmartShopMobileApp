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
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.ViewModels
{
    public partial class LogInViewModel:ObservableObject
    {
        public LogInViewModel()
        {
            _manageData = new ManageData();
            if (AuthenticationResultHelper.ActiveUser == null)
            {
                AuthenticationResultHelper.ActiveUser = new UserDTO();
            }

            AuthenticationResultHelper.ActiveUser.UserID = 1;
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
                _manageData.SetStrategy(new GetData());

                AuthenticationResultHelper.ActiveUser = await _manageData.GetDataAndDeserializeIt<UserDTO>($"User/GetUserByEmailAndPassword?email={Email}&password={Password}", "");
                Application.Current.MainPage =  new AppShell();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [RelayCommand]
        private async Task OpenSignUpPage()
        {
            try
            {
                App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new SignUpView()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
