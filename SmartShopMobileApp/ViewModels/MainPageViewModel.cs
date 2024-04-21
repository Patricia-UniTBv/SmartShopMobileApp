using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {


        public MainPageViewModel()
        {

        }

        [RelayCommand]
        private async Task LogIn()
        {
            await Shell.Current.GoToAsync("//LogInView");
        }

        [RelayCommand]
        private async Task SignUp()
        {
            await Shell.Current.GoToAsync("//SignUpView");
        }
    }
}
