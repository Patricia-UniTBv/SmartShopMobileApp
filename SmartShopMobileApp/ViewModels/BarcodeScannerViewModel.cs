using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using Newtonsoft.Json;
using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.Services;
using SmartShopMobileApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
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
            _authService = new AuthService();

            ActiveUser = new AuthResponseDTO();

            IdentifyProductByBarcode(barcode);
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

        [ObservableProperty]
        public string _productName;

        [ObservableProperty]
        public string _barcodeResult;

        [ObservableProperty]
        public int _numberOfProducts;

        [ObservableProperty]
        private AuthResponseDTO _activeUser;

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
            var result = await _manageData.GetDataAndDeserializeIt<object>($"Product/AddProductToShoppingCart?productId={Product.ProductID}&numberOfProducts={NumberOfProducts}" +
                $"&supermarketId={CurrentSupermarket.Supermarket.SupermarketID}&userId={ActiveUser.UserId}", json);
            
            if(result != null)
                await Application.Current.MainPage.DisplayAlert("Successfully added", "Your product has been successfully added to the shopping cart!", "OK");

        }

        [RelayCommand]
        public async Task AddNumberOfProducts(object obj)
        {
            NumberOfProducts++;
        }
    }
}
