using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Views;
using DTO;
using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.Views;
using Newtonsoft.Json;

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

        public string SupermarketInputName { get; set; }

        #region ObservableProperties
       
        [ObservableProperty]
        private List<SupermarketDTO> _supermarkets;
        #endregion

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
        }

        [RelayCommand]
        private async Task AddButton(object obj)
        {

            string input = await Application.Current.MainPage.DisplayPromptAsync("Add supermarket", "What's its name?");
            SupermarketDTO newSupermarket = new SupermarketDTO();
            newSupermarket.Name = input;

            var _httpClient = new HttpClient(App.Current.MainPage.Handler.MauiContext.Services.GetService<IHttpClientHandlerService>().GetInsecureHandler());

            var json = JsonConvert.SerializeObject(newSupermarket);

            _manageData.SetStrategy(new CreateData());
            var result = await _manageData.GetDataAndDeserializeIt<SupermarketDTO>($"Supermarket/AddSupermarket", json);

            Supermarkets = await _manageData.GetDataAndDeserializeIt<List<SupermarketDTO>>($"Supermarket/GetAllSupermarkets", "");
            await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new HomeView(Supermarkets)));

        }
        #endregion
    }
}
