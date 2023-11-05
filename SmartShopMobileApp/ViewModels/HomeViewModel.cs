//using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartShopMobileApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.ViewModels
{
    public partial class HomeViewModel: ObservableObject
    {
        public HomeViewModel() { }

        [RelayCommand]
        private async Task OpenScanner()
        {
            try
            {
                await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new BarcodeScanner()));
                //await App.Current.MainPage.Navigation.PopToRootAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
