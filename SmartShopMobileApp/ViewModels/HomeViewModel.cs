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
            Products = new List<ProductDTO>();
            GetProducts();
        }

        private IManageData _manageData;
        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
        }

        #region ObservableProperties
        [ObservableProperty]
        private List<ProductDTO> _products;

        private async Task GetProducts()
        {
            _manageData.SetStrategy(new GetData());
            Products = await _manageData.GetDataAndDeserializeIt<List<ProductDTO>>("Product/GetAllProducts", "");      
        }
        #endregion

        #region RelayCommands
        [RelayCommand]
        private async Task OpenScanner()
        {
            try
            {
                //var popup = new ScannedProductPopupView();
                await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new BarcodeScanner()));
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        #endregion
    }
}
