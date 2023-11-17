using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using Newtonsoft.Json;
using SmartShopMobileApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.QrCode.Internal;

namespace SmartShopMobileApp.ViewModels
{
    public partial class BarcodeScannerViewModel: ObservableObject, INotifyPropertyChanged
    {
        public BarcodeScannerViewModel(string barcode) 
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

        private string _barcodeResult;

        public string BarcodeResult
        {
            get { return _barcodeResult; }
            set
            {
                if (_barcodeResult != value)
                {
                    _barcodeResult = value;
                    OnPropertyChanged(nameof(BarcodeResult));
                }
            }
        }
        //[ObservableProperty]
        //public string BarcodeResult;

        public ProductDTO Product { get; set; }

        [RelayCommand]
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
            var _httpClient = new HttpClient(App.Current.MainPage.Handler.MauiContext.Services.GetService<IHttpClientHandlerService>().GetInsecureHandler());

            var json = JsonConvert.SerializeObject(Product);

            _manageData.SetStrategy(new CreateData());
            var result = await _manageData.GetDataAndDeserializeIt<ProductDTO>("Product/AddProductToShoppingCart", json);

        }
    }
}
