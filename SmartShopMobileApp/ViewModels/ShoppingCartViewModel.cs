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
            Products = new List<ProductDTO>();
            _userId = 1; //provizoriu
            GetLatestShoppingCartForCurrentUser();
        }

        private IManageData _manageData;
        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
        }
       
        private int _userId { get; set; }//provizoriu

        [ObservableProperty]
        private List<ProductDTO> _products;

       // [ObservableProperty]
        private ProductDTO _selectedProduct;
        public ProductDTO SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    OnPropertyChanged(nameof(SelectedProduct));
                }
            }
        }

        [RelayCommand]
        private async Task DeleteCartItem(object obj)
        {
            try
            {
                // var x = SelectedProduct.ProductId;
                int id = (int)obj;

                _manageData.SetStrategy(new GetData());
                var latestShoppingCart = await _manageData.GetDataAndDeserializeIt<ShoppingCartDTO>($"ShoppingCart/GetLatestShoppingCartByUserId?id={_userId}", "");
                
                _manageData.SetStrategy(new DeleteData());
                await _manageData.GetDataAndDeserializeIt<ShoppingCartDTO>($"CartItem/DeleteCartItemFromShoppingCart?productId={id}&shoppingCartId={latestShoppingCart.ShoppingCartID}", "");
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
                Products = await _manageData.GetDataAndDeserializeIt<List<ProductDTO>>($"ShoppingCart/GetLatestShoppingCartForCurrentUser?id={_userId}", "");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
