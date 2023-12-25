﻿using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class HistoryAndStatisticsViewModel: ObservableObject
    {
        public HistoryAndStatisticsViewModel() 
        {
            _manageData = new ManageData();
            if (AuthenticationResultHelper.ActiveUser == null)
            {
                AuthenticationResultHelper.ActiveUser = new UserDTO();
            }

            AuthenticationResultHelper.ActiveUser.UserID = 1;
        }
        private IManageData _manageData;
        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
        }

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
        private async Task PageAppearing(object obj)
        {
            await GetAllShoppingCarts();
        }
    }
}
