using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Views;
using DTO;
using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.Views;
using System.Collections.ObjectModel;

namespace SmartShopMobileApp.ViewModels
{
    public partial class HomeViewModel: ObservableObject
    {
        public HomeViewModel() 
        {
            _manageData = new ManageData();
            Supermarkets = new List<SupermarketDTO>();
            IsCurrentOffersVisible = false;

            GetCurrentOffers();
        }

        private IManageData _manageData;
        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
        }

        [ObservableProperty]
        private bool _isCurrentOffersVisible;

        [ObservableProperty]
        private ObservableCollection<OfferDTO> _currentOffers;

        private async Task GetCurrentOffers()
        {
            try
            {
                _manageData.SetStrategy(new GetData());
                var offers = await _manageData.GetDataAndDeserializeIt<ObservableCollection<OfferDTO>>("Offer/GetAllCurrentOffers", "");
                if (offers.Count > 0)
                {
                    IsCurrentOffersVisible = true;
                    foreach(var offer in offers) 
                    {
                        offer.OldPrice = offer.Product.Price;
                        offer.NewPrice = offer.Product.Price * (1 - (decimal)offer.OfferPercentage / 100);
                    }
                }
                CurrentOffers = offers;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


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
            
            App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new MapView(selectedSupermarket.SupermarketID)));
        }

        [RelayCommand]
        private async void PageAppearing(object obj)
        {
            GetSupermarkets();
        }

    }
}
