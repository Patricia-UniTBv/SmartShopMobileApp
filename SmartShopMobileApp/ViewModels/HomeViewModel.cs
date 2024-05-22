using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Views;
using DTO;
using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.Views;
using System.Collections.ObjectModel;
using SmartShopMobileApp.Services;
using SmartShopMobileApp.Services.Interfaces;
using SmartShopMobileApp.Resources.Languages;
using System.Globalization;
using MailKit.Search;
using System.Drawing.Printing;
using CommunityToolkit.Maui.Core.Extensions;

namespace SmartShopMobileApp.ViewModels
{
    public partial class HomeViewModel: ObservableObject
    {
        public HomeViewModel() 
        {
            _manageData = new ManageData();
           
            ActiveUser = AuthenticatedUser.ActiveUser;

            Supermarkets = new List<SupermarketDTO>();
           
            IsCurrentOffersVisible = false;
        }

        private IManageData _manageData;
        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
        }

        private readonly CurrencyConversionService _conversionService = new CurrencyConversionService();

        [ObservableProperty]
        private bool _isCurrentOffersVisible;

        [ObservableProperty]
        private ObservableCollection<OfferDTO> _currentOffers;

        [ObservableProperty]
        private ObservableCollection<OfferDTO> _allCurrentOffers;

        [ObservableProperty]
        private string _currency;

        private string _searchedOffer;
        public string SearchedOffer
        {
            get => _searchedOffer;
            set
            {
                _searchedOffer = value;
                OnPropertyChanged();
                FilterOffers();
            }
        }

        private int numberOfOffers { get; set; }

        [ObservableProperty]
        private AuthResponseDTO _activeUser = new();
        private async Task GetCurrentOffers()
        {
            try
            {
                _manageData.SetStrategy(new GetData());
                var offers = await _manageData.GetDataAndDeserializeIt<ObservableCollection<OfferDTO>>("Offer/GetAllCurrentOffers", "");
                if (offers.Count > 0)
                {
                    IsCurrentOffersVisible = true;
                    foreach (var offer in offers)
                    {
                        offer.OldPrice = Math.Round(_conversionService.ConvertCurrencyAsync(offer.Product.Price, "RON", PreferredCurrency.Value), 2);
                        decimal fromPrice = Math.Round(offer.Product.Price * (1 - (decimal)offer.OfferPercentage / 100), 2);
                        offer.NewPrice = Math.Round(_conversionService.ConvertCurrencyAsync(fromPrice, "RON", PreferredCurrency.Value), 2);
                        offer.Currency = PreferredCurrency.Value switch
                        {
                            "USD" => "$",
                            "EUR" => "€",
                            "GBP" => "£",
                            _ => PreferredCurrency.Value,
                        };
                    }

                }
                var list = offers.Take(4).ToList();
                CurrentOffers = new ObservableCollection<OfferDTO>(list);
                AllCurrentOffers = offers;
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

        private void FilterOffers()
        {
            if (!string.IsNullOrWhiteSpace(SearchedOffer))
            {
                var query = SearchedOffer.ToLower();
                CurrentOffers = new ObservableCollection<OfferDTO>(AllCurrentOffers.Where(o => o.Product.Name.ToLower().Contains(query)));
            }
        }

        [RelayCommand]
        private async Task OpenScanner()
        {
            try
            {
                await App.Current.MainPage.Navigation.PushAsync(new BarcodeScannerView());

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        [RelayCommand]
        private async Task LoadMore()
        {
            try
            {
                var nextOffers = AllCurrentOffers.Skip(numberOfOffers).Take(numberOfOffers).ToList();
                foreach (var offer in nextOffers)
                {
                    CurrentOffers.Add(offer);
                }
                numberOfOffers += 4;
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
            
            App.Current.MainPage.Navigation.PushAsync(new MapView(selectedSupermarket.SupermarketID));
        }

        [RelayCommand]
        private async Task PageAppearing(object obj)
        {
            numberOfOffers = 4;
            await GetSupermarkets();
            await GetCurrentOffers();
        }

    }
}
