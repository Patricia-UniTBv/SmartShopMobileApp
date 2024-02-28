using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.ViewModels
{
    public partial class LogInViewModel:ObservableObject
    {
        [RelayCommand]
        private async Task LogIn()
        {
            try
            {
                Application.Current.MainPage =  new AppShell();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
