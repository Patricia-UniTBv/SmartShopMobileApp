using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using SmartShopMobileApp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.ViewModels
{
    public partial class ShoppingCartItemsViewModel: ObservableObject   
    {
        public ShoppingCartItemsViewModel(ShoppingCartDTO selectedCart) 
        {
            _manageData = new ManageData();
            shoppingCart = selectedCart;
        }
        private IManageData _manageData;
        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
        }

        private ShoppingCartDTO shoppingCart { get; set; }

        [ObservableProperty]
        private ObservableCollection<ProductDTO> _cartItems;

        private async Task GetItemsForShoppingCart()
        {
            try
            {
                _manageData.SetStrategy(new GetData());

                CartItems = await _manageData.GetDataAndDeserializeIt<ObservableCollection<ProductDTO>>($"CartItem/GetItemsForShoppingCart?shoppingCartId={shoppingCart.ShoppingCartID}", "");                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [RelayCommand]
        private async Task PageAppearing(object obj)
        {
            await GetItemsForShoppingCart();
        }
    }
}
