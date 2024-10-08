﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.Services;
using SmartShopMobileApp.Services.Interfaces;
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

            ActiveUser = AuthenticatedUser.ActiveUser;

            IsDataFiltered = false;
        }

     
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

        [ObservableProperty]
        private AuthResponseDTO _activeUser = new();

        private async Task GetAllShoppingCarts()
        {
            try
            {
                _manageData.SetStrategy(new GetData());

                ShoppingCarts = await _manageData.GetDataAndDeserializeIt<ObservableCollection<ShoppingCartDTO>>($"ShoppingCart/GetAllTransactedShoppingCartsWithSupermarketByUserId?id={ActiveUser.UserId}", "");
                ShoppingCarts = new ObservableCollection<ShoppingCartDTO>(ShoppingCarts.OrderByDescending(s => s.CreationDate));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [RelayCommand]
        private static void OnShoppingCartSelected(ShoppingCartDTO selectedCart)
        {
            App.Current.MainPage.Navigation.PushAsync(new ShoppingCartItemsView(selectedCart));
        }

        [RelayCommand]
        private async void FilterShoppingCarts(object obj)
        {
            _manageData.SetStrategy(new GetData());

            ShoppingCarts = await _manageData.GetDataAndDeserializeIt<ObservableCollection<ShoppingCartDTO>>($"ShoppingCart/GetAllTransactedShoppingCartsWithSupermarketByUserId?id={ActiveUser.UserId}", "");

            ShoppingCarts = new ObservableCollection<ShoppingCartDTO>(
                                ShoppingCarts
                                    .Where(item =>
                                        item.CreationDate >= StartDate &&
                                        item.CreationDate <= EndDate &&
                                        item.TotalAmount >= MinPrice &&
                                        item.TotalAmount <= MaxPrice &&
                                        (SelectedStore == null || item.Supermarket?.Name == SelectedStore))
                                    .ToList());
        }

        [RelayCommand]
        private void ShowFilters(object obj)
        {
            IsFilterGridVisible = !IsFilterGridVisible;
        }

        [RelayCommand]
        private void ViewStatistics(object obj)
        {
            App.Current.MainPage.Navigation.PushAsync(new MonthlySpendingsView());
        }

        [RelayCommand]
        private async Task PageAppearing(object obj)
        {
            MinPrice = 0;
            MaxPrice = 1000;
            StartDate = DateTime.Now.AddMonths(-12);
            EndDate = DateTime.Now;

            if (IsDataFiltered == false)
            {
                await GetAllShoppingCarts();
            }
        }
    }
}
