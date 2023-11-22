using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class ShoppingCartViewModel: ObservableObject
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
