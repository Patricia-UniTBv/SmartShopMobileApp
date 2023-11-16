using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using Newtonsoft.Json;
using SmartShopMobileApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZXing.QrCode.Internal;

namespace SmartShopMobileApp.ViewModels
{
    public partial class ScannedProductPopupViewModel: Popup, INotifyPropertyChanged
    {
        public ScannedProductPopupViewModel(string barcode)
        {
            _manageData = new ManageData();
            IdentifyProductByBarcode(barcode);
        }
        private IManageData _manageData;
        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
        }

        private string _productName;

        public string ProductName
        {
            get { return _productName; }
            set
            {
                if (_productName != value)
                {
                    _productName = value;
                    OnPropertyChanged(nameof(ProductName));
                }
            }
        }

        public ProductDTO Product { get; set; }

        public async Task IdentifyProductByBarcode(string barcode)
        {
            try
            {
                _manageData.SetStrategy(new GetData());
                Product = await _manageData.GetDataAndDeserializeIt<ProductDTO>($"Product/GetProductByBarcode/{barcode}", "");
                ProductName = Product.Name;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [RelayCommand]
        public async Task AddProductToCart(object obj)
        {
            HttpClient client = new HttpClient(DependencyService.Get<IHttpClientHandlerService>().GetInsecureHandler());

            var json = JsonConvert.SerializeObject(Product);

            _manageData.SetStrategy(new CreateData());
            await _manageData.GetDataAndDeserializeIt<object>("Product/AddProductToShoppingCart", json);

        }

    }
}
