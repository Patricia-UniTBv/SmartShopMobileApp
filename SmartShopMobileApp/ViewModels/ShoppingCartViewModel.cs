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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.ViewModels
{
    public partial class ShoppingCartViewModel: ObservableObject, INotifyPropertyChanged
    {
        public ShoppingCartViewModel() 
        {
            _manageData = new ManageData();
            _authService = new AuthService();

            ActiveUser = new AuthResponseDTO();
            ActiveUser = AuthenticatedUser.ActiveUser;

            Products = new List<ProductDTO>() { };

            Currency = PreferredCurrency.Value;
        }

        private IManageData _manageData;
        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
        }

        private IAuthService _authService;
        public IAuthService AuthService
        {
            get { return _authService; }
            set { _authService = value; }
        }

        private readonly CurrencyConversionService _conversionService = new CurrencyConversionService();
        public int ShoppingCartId { get; set; }

        public decimal VoucherDiscount { get;set; }

        [ObservableProperty]
        public bool _isEmptyShoppingCartTextVisible;

        [ObservableProperty]
        public bool _isVoucherButtonVisible;

        [ObservableProperty]
        public string _imageSource;

        [ObservableProperty]
        private List<ProductDTO> _products;

        [ObservableProperty]
        private decimal _totalAmount;

        [ObservableProperty]
        private string _currency;

        [ObservableProperty]
        private AuthResponseDTO _activeUser;


        [RelayCommand]
        private async Task DeleteCartItem(object obj)
        {
            try
            {
                ProductDTO product = (ProductDTO)obj;

                _manageData.SetStrategy(new GetData());
                var latestShoppingCart = await _manageData.GetDataAndDeserializeIt<ShoppingCartDTO>($"ShoppingCart/GetLatestShoppingCartByUserId?id={ActiveUser.UserId}", "");
                
                _manageData.SetStrategy(new DeleteData());
                await _manageData.GetDataAndDeserializeIt<ShoppingCartDTO>($"CartItem/DeleteCartItemFromShoppingCart?productId={product.ProductID}&shoppingCartId={latestShoppingCart.ShoppingCartID}&quantity={product.Quantity}", "");
                await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new ShoppingCartView()));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private async Task GetLatestShoppingCartForCurrentUser()
        {
            try
            {
                _manageData.SetStrategy(new GetData());

                var latestShoppingCart = await _manageData.GetDataAndDeserializeIt<ShoppingCartDTO>($"ShoppingCart/GetLatestShoppingCartByUserId?id={ActiveUser.UserId}", "");

                if (latestShoppingCart != null)
                {
                    Products = await _manageData.GetDataAndDeserializeIt<List<ProductDTO>>($"ShoppingCart/GetLatestShoppingCartForCurrentUser?id={ActiveUser.UserId}", "");
                    foreach(var product in Products)
                    {
                        product.Price = Math.Round(_conversionService.ConvertCurrencyAsync(product.Price, "RON", PreferredCurrency.Value), 2);
                        product.Currency = Currency;
                    }
                    ShoppingCartId = latestShoppingCart.ShoppingCartID;
                    TotalAmount = Math.Round(_conversionService.ConvertCurrencyAsync(latestShoppingCart.TotalAmount, "RON", PreferredCurrency.Value), 2);


                    if (ShoppingCartId != 0)
                    {
                        ImageSource = "shopping_cart.png";
                        IsVoucherButtonVisible = true;
                    }
                    else
                    {
                        ImageSource = "cartempty.png";
                        IsEmptyShoppingCartTextVisible = true;
                        IsVoucherButtonVisible = false;
                    }
                }
                else
                {
                    ImageSource = "cartempty.png";
                    IsEmptyShoppingCartTextVisible = true;
                    IsVoucherButtonVisible = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [RelayCommand]
        private async Task Pay(object obj)
        {
            try
            {
                await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new PaymentView(TotalAmount, ShoppingCartId, VoucherDiscount)));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        [RelayCommand]
        private async Task ApplyVoucher(object obj)
        {
            try
            {
                if(CurrentSupermarket.Supermarket != null)
                    await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new VoucherView()));
                else
                {
                    await Application.Current.MainPage.DisplayAlert("", "Please select a supermarket", "OK");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [RelayCommand]
        private async Task PageAppearing(object obj)
        {
            IsEmptyShoppingCartTextVisible = false;
            await GetLatestShoppingCartForCurrentUser();
        }

    }
}
