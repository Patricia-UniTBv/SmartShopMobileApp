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
        }

        private IManageData _manageData;
        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
        }
       
        public int ShoppingCartId { get; set; }
        private int _userId { get; set; }//provizoriu

        [ObservableProperty]
        public bool _isEmptyShoppingCartTextVisible;

        [ObservableProperty]
        public string _imageSource;

        [ObservableProperty]
        private List<ProductDTO> _products;

        [ObservableProperty]
        private double _totalAmount;

        [RelayCommand]
        private async Task DeleteCartItem(object obj)
        {
            try
            {
                ProductDTO product = (ProductDTO)obj;

                _manageData.SetStrategy(new GetData());
                var latestShoppingCart = await _manageData.GetDataAndDeserializeIt<ShoppingCartDTO>($"ShoppingCart/GetLatestShoppingCartByUserId?id={_userId}", "");
                
                _manageData.SetStrategy(new DeleteData());
                await _manageData.GetDataAndDeserializeIt<ShoppingCartDTO>($"CartItem/DeleteCartItemFromShoppingCart?productId={product.ProductId}&shoppingCartId={latestShoppingCart.ShoppingCartID}&quantity={product.Quantity}", "");
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

                    ShoppingCartId = latestShoppingCart.ShoppingCartID;
                    TotalAmount = latestShoppingCart.TotalAmount;

                    if (ShoppingCartId != 0)
                    {
                        ImageSource = "shopping_cart.png";
                    }
                    else
                    {
                        ImageSource = "cartempty.png";
                        IsEmptyShoppingCartTextVisible = true;
                    }
                }
                else
                {
                    ImageSource = "cartempty.png";
                    IsEmptyShoppingCartTextVisible = true;
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
                await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new PaymentView(TotalAmount, ShoppingCartId)));

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
        private async void PageAppearing(object obj)
        {
            IsEmptyShoppingCartTextVisible = false;
            GetLatestShoppingCartForCurrentUser();
        }

    }
}
