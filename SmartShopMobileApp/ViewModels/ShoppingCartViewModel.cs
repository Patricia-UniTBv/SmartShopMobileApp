using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using SmartShopMobileApp.Helpers;
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
            Products = new List<ProductDTO>() { };
            _userId = 1; //provizoriu

            if (AuthenticationResultHelper.ActiveUser == null)
            {
                AuthenticationResultHelper.ActiveUser = new UserDTO();
            }

            AuthenticationResultHelper.ActiveUser.UserID = 1;
            AuthenticationResultHelper.ActiveUser.PreferredCurrency = "EUR";

            Currency = AuthenticationResultHelper.ActiveUser.PreferredCurrency.ToLower();
        }

        private IManageData _manageData;
        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
        }

        private readonly CurrencyConversionService _conversionService = new CurrencyConversionService();
        public int ShoppingCartId { get; set; }
        private int _userId { get; set; }//provizoriu

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


        [RelayCommand]
        private async Task DeleteCartItem(object obj)
        {
            try
            {
                ProductDTO product = (ProductDTO)obj;

                _manageData.SetStrategy(new GetData());
                var latestShoppingCart = await _manageData.GetDataAndDeserializeIt<ShoppingCartDTO>($"ShoppingCart/GetLatestShoppingCartByUserId?id={_userId}", "");
                
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

                var latestShoppingCart = await _manageData.GetDataAndDeserializeIt<ShoppingCartDTO>($"ShoppingCart/GetLatestShoppingCartByUserId?id={_userId}", "");

                if (latestShoppingCart != null)
                {
                    Products = await _manageData.GetDataAndDeserializeIt<List<ProductDTO>>($"ShoppingCart/GetLatestShoppingCartForCurrentUser?id={_userId}", "");
                    foreach(var product in Products)
                    {
                        product.Price = Math.Round(_conversionService.ConvertCurrencyAsync(product.Price, "RON", AuthenticationResultHelper.ActiveUser.PreferredCurrency), 2);
                        product.Currency = Currency;
                    }
                    ShoppingCartId = latestShoppingCart.ShoppingCartID;
                    TotalAmount = Math.Round(_conversionService.ConvertCurrencyAsync(latestShoppingCart.TotalAmount, "RON", AuthenticationResultHelper.ActiveUser.PreferredCurrency), 2);


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
