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
        }

        private IManageData _manageData;
        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
        }
       
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

        [RelayCommand]
        private async Task DeleteCartItem(object obj)
        {
            try
            {
                ProductDTO product = (ProductDTO)obj;

                _manageData.SetStrategy(new GetData());
                var latestShoppingCart = await _manageData.GetDataAndDeserializeIt<ShoppingCartDTO>($"ShoppingCart/GetLatestShoppingCartByUserId?id={AuthenticationResultHelper.ActiveUser.UserID}", "");
                
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

                var latestShoppingCart = await _manageData.GetDataAndDeserializeIt<ShoppingCartDTO>($"ShoppingCart/GetLatestShoppingCartByUserId?id={AuthenticationResultHelper.ActiveUser.UserID}", "");

                if (latestShoppingCart != null)
                {
                    Products = await _manageData.GetDataAndDeserializeIt<List<ProductDTO>>($"ShoppingCart/GetLatestShoppingCartForCurrentUser?id={AuthenticationResultHelper.ActiveUser.UserID}", "");

                    ShoppingCartId = latestShoppingCart.ShoppingCartID;
                    TotalAmount = latestShoppingCart.TotalAmount;

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
                await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new VoucherView()));

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
