using CommunityToolkit.Mvvm.Input;
using SmartShopMobileApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.ViewModels
{
    public partial class GeneratedQRCodeToExitShopViewModel
    {
        public GeneratedQRCodeToExitShopViewModel()
        {

        }

        [RelayCommand]
        private async Task FinishShopping(object obj)
        {
            try
            {
                await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new HomeView()));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
