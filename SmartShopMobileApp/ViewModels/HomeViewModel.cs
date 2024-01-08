using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Views;
using DTO;
using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.Views;

namespace SmartShopMobileApp.ViewModels
{
    public partial class HomeViewModel: ObservableObject
    {
        public HomeViewModel() 
        {
            _manageData = new ManageData();
            Supermarkets = new List<SupermarketDTO>();
            GetSupermarkets();
        }

        private IManageData _manageData;
        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
        }

        #region ObservableProperties
       
        [ObservableProperty]
        private List<SupermarketDTO> _supermarkets;

        private async Task GetSupermarkets()
        {
            try
            {
                _manageData.SetStrategy(new GetData());
                Supermarkets = await _manageData.GetDataAndDeserializeIt<List<SupermarketDTO>>($"Supermarket/GetAllSupermarkets", "");
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion

        #region RelayCommands
        [RelayCommand]
        private async Task OpenScanner()
        {
            try
            {
                await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new BarcodeScannerView()));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        [RelayCommand]
        private void OnSupermarketSelected(SupermarketDTO selectedSupermarket)
        {
            CurrentSupermarket.Supermarket = selectedSupermarket;
            Application.Current.MainPage.DisplayAlert("Successfully selected", $"You've selected {selectedSupermarket.Name}", "OK");
            Thread.Sleep(1000);
            App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new BarcodeScannerView()));
        }
        #endregion
    }
}
