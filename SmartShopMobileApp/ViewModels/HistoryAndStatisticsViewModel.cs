using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
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
    public partial class HistoryAndStatisticsViewModel : ObservableObject
    {
        public HistoryAndStatisticsViewModel()
        {
            _manageData = new ManageData();
            IsDataFiltered = false;
            if (AuthenticationResultHelper.ActiveUser == null)
            {
                AuthenticationResultHelper.ActiveUser = new UserDTO();
            }

            AuthenticationResultHelper.ActiveUser.UserID = 1;
        }

        //public HistoryAndStatisticsViewModel(ObservableCollection<ShoppingCartDTO> carts)
        //{
        //    _manageData = new ManageData();
        //    ShoppingCarts = carts;
        //    IsDataFiltered = true;
        //    IsFilterGridVisible = true;
        //    if (AuthenticationResultHelper.ActiveUser == null)
        //    {
        //        AuthenticationResultHelper.ActiveUser = new UserDTO();
        //    }

        //    AuthenticationResultHelper.ActiveUser.UserID = 1;
        //}
        private IManageData _manageData;
        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
        }

        public bool IsDataFiltered { get; set; }

        [ObservableProperty]
        public bool _isFilterGridVisible;

        [ObservableProperty]
        private DateTime _startDate;

        [ObservableProperty]
        private DateTime _endDate;

        [ObservableProperty]
        private decimal _minPrice;

        [ObservableProperty]
        private decimal _maxPrice;

        [ObservableProperty]
        private string _selectedStore;

        [ObservableProperty]
        public ObservableCollection<ShoppingCartDTO> shoppingCarts;

        private async Task GetAllShoppingCarts()
        {
            try
            {
                _manageData.SetStrategy(new GetData());

                ShoppingCarts = await _manageData.GetDataAndDeserializeIt<ObservableCollection<ShoppingCartDTO>>($"ShoppingCart/GetAllTransactedShoppingCartsWithSupermarketByUserId?id={AuthenticationResultHelper.ActiveUser.UserID}", "");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [RelayCommand]
        private static void OnShoppingCartSelected(ShoppingCartDTO selectedCart)
        {
            App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new ShoppingCartItemsView(selectedCart)));
        }

        [RelayCommand]
        private async void FilterShoppingCarts(object obj)
        {
            _manageData.SetStrategy(new GetData());

            ShoppingCarts = await _manageData.GetDataAndDeserializeIt<ObservableCollection<ShoppingCartDTO>>($"ShoppingCart/GetAllTransactedShoppingCartsWithSupermarketByUserId?id={AuthenticationResultHelper.ActiveUser.UserID}", "");

            ShoppingCarts = new ObservableCollection<ShoppingCartDTO>(
                                ShoppingCarts
                                    .Where(item =>
                                        item.CreationDate >= StartDate &&
                                        item.CreationDate <= EndDate &&
                                        item.TotalAmount >= MinPrice &&
                                        item.TotalAmount <= MaxPrice &&
                                        (SelectedStore == null || item.Supermarket?.Name == SelectedStore))
                                    .ToList()
                            );

        }

        [RelayCommand]
        private void ShowFilters(object obj)
        {
            //FilterGrid.IsVisible = !FilterGrid.IsVisible;
            IsFilterGridVisible = !IsFilterGridVisible;
        }

        [RelayCommand]
        private async Task PageAppearing(object obj)
        {
            MinPrice = 0;
            MaxPrice = 1000;
            StartDate = DateTime.Now.AddMonths(-12);
            EndDate = DateTime.Now;
            if(IsDataFiltered == false)
              await GetAllShoppingCarts();
        }
    }
}
