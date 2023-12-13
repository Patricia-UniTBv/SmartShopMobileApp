using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using Newtonsoft.Json;
using SmartShopMobileApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.ViewModels
{
    public partial class VoucherViewModel: ObservableObject
    {
        public VoucherViewModel() 
        {
            _manageData = new ManageData();
            if (AuthenticationResultHelper.ActiveUser == null)
            {
                AuthenticationResultHelper.ActiveUser = new UserDTO();
            }

            AuthenticationResultHelper.ActiveUser.UserID = 1;
        }

        private IManageData _manageData;
        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
        }

        [ObservableProperty]
        public string _earnedMoneyText;

        [ObservableProperty]
        public string _noMoneyOnVoucherText;

        [ObservableProperty]
        public bool _isApplyButtonEnabled;

        [ObservableProperty]
        public string _applyButtonColor;

        [ObservableProperty]
        public string _currentSupermarketName;

        [ObservableProperty]
        public int? _earnedMoney;

        private async Task GetEarnedMoney()
        {
            try
            {
                _manageData.SetStrategy(new GetData());
                var voucher = await _manageData.GetDataAndDeserializeIt<VoucherDTO>($"Voucher/GetVoucherForUserAndSupermarket/{AuthenticationResultHelper.ActiveUser.UserID}/{CurrentSupermarket.Supermarket.SupermarketID}", "");
                EarnedMoneyText =  $"Total: {voucher.EarnedPoints} lei";
                EarnedMoney = voucher.EarnedPoints;

                if (voucher.EarnedPoints == 0)
                {
                    NoMoneyOnVoucherText = "You currently don't have any money on your voucher...";
                    IsApplyButtonEnabled = false;
                    ApplyButtonColor = "Gray";
                }
                else
                {
                    NoMoneyOnVoucherText = "";
                    IsApplyButtonEnabled = false;
                    ApplyButtonColor = "Gray";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [RelayCommand]
        private async Task ApplyVoucher()
        {
            try
            {
                _manageData.SetStrategy(new GetData());
                var latestShoppingCart = await _manageData.GetDataAndDeserializeIt<ShoppingCartDTO>($"ShoppingCart/GetLatestShoppingCartByUserId?id={AuthenticationResultHelper.ActiveUser.UserID}", "");
                latestShoppingCart.TotalAmount -= Convert.ToDouble(EarnedMoney);

                _manageData.SetStrategy(new UpdateData());
                var json = JsonConvert.SerializeObject(latestShoppingCart);
                await _manageData.GetDataAndDeserializeIt<object>($"ShoppingCart/UpdateShoppingCart", json);

                _manageData.SetStrategy(new UpdateData());
                await _manageData.GetDataAndDeserializeIt<object>($"Voucher/UpdateVoucherForSpecificUserWhenItIsUsed/{AuthenticationResultHelper.ActiveUser.UserID}/{CurrentSupermarket.Supermarket.SupermarketID}", "");

                await App.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [RelayCommand]
        private async void PageAppearing(object obj)
        {
            CurrentSupermarketName = CurrentSupermarket.Supermarket.Name;
            await GetEarnedMoney();
        }

    }
}
