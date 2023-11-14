using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using SmartShopMobileApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.ViewModels
{
    public class ScannedProductPopupViewModel: Popup, INotifyPropertyChanged
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

        public async Task IdentifyProductByBarcode(string barcode)
        {
            try
            {
                _manageData.SetStrategy(new GetData());
                var product = await _manageData.GetDataAndDeserializeIt<ProductDTO>($"Product/GetProductByBarcode/{barcode}", "");
                ProductName = product.Name;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
