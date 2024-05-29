using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using Newtonsoft.Json;
using SmartShopMobileApp.HelperModels;
using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.Services;
using SmartShopMobileApp.Services.Interfaces;
using SmartShopMobileApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.ViewModels
{
    public partial class VoucherViewModel : ObservableObject
    {
        public VoucherViewModel()
        {
            _manageData = new ManageData();

            ActiveUser = AuthenticatedUser.ActiveUser;

            VouchersHistory = new ObservableCollection<VoucherHistory>();
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
        public decimal? _earnedMoney;

        [ObservableProperty]
        public ObservableCollection<VoucherHistory> _vouchersHistory;

        [ObservableProperty]
        private AuthResponseDTO _activeUser = new();

        private async Task GetEarnedMoney()
        {
            try
            {
                _manageData.SetStrategy(new GetData());
                var voucher = await _manageData.GetDataAndDeserializeIt<VoucherDTO>($"Voucher/GetVoucherForUserAndSupermarket/{ActiveUser.UserId}/{CurrentSupermarket.Supermarket.SupermarketID}", "");
                EarnedMoneyText = $"Total: {voucher.EarnedPoints} lei";
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
                    IsApplyButtonEnabled = true;
                    ApplyButtonColor = "#2B0B98";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task GetVoucherHistory()
        {
            try
            {
                _manageData.SetStrategy(new GetData());
                VouchersHistory = new ObservableCollection<VoucherHistory>();
                var allShoppingCarts = await _manageData.GetDataAndDeserializeIt<List<ShoppingCartDTO>>($"ShoppingCart/GetAllTransactedShoppingCartsByUserId?id={ActiveUser.UserId}&supermarketId={CurrentSupermarket.Supermarket.SupermarketID}", "");
                foreach (var cart in allShoppingCarts)
                {
                    if (cart.TotalAmount > 150) 
                    {
                        var newVoucherHistory = new VoucherHistory();
                        newVoucherHistory.CartCreationDate = cart.CreationDate.ToShortDateString();
                        newVoucherHistory.CreationDate = cart.CreationDate;
                        newVoucherHistory.TotalAmount = cart.TotalAmount.ToString("F2");
                        newVoucherHistory.ValueModification = "+ " + (0.05m * cart.TotalAmount).ToString().Substring(0,4) + " lei";
                        newVoucherHistory.TextColor = Colors.Green;
                        VouchersHistory.Add(newVoucherHistory);
                    }
                 
                }

                var usedDiscounts = await _manageData.GetDataAndDeserializeIt<List<TransactionDTO>>($"Transaction/GetAllTransactionsWithDiscount/{ActiveUser.UserId}/{CurrentSupermarket.Supermarket.SupermarketID}", "");
                if (usedDiscounts.Count != 0)
                {
                    foreach (var transaction in usedDiscounts)
                    {
                        if (transaction.VoucherDiscount != 0)
                        {
                            var newVoucherHistory = new VoucherHistory();
                            newVoucherHistory.CartCreationDate = transaction.TransactionDate.ToShortDateString();
                            newVoucherHistory.CreationDate = transaction.TransactionDate;
                            newVoucherHistory.ValueModification = "- " + (transaction.VoucherDiscount).ToString() + " lei";
                            newVoucherHistory.TotalAmount = transaction.TotalAmount.ToString("F2");
                            newVoucherHistory.TextColor = Colors.Red;
                            VouchersHistory.Add(newVoucherHistory);
                        }
                    }
                }

                var list = VouchersHistory.OrderByDescending(t => t.CreationDate).ToObservableCollection();
                VouchersHistory = list;

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
                var latestShoppingCart = await _manageData.GetDataAndDeserializeIt<ShoppingCartDTO>($"ShoppingCart/GetLatestShoppingCartByUserId?id={ActiveUser.UserId}", "");
                latestShoppingCart.TotalAmount -= Convert.ToDecimal(EarnedMoney);

                _manageData.SetStrategy(new UpdateData());
                var json = JsonConvert.SerializeObject(latestShoppingCart);
                await _manageData.GetDataAndDeserializeIt<object>($"ShoppingCart/UpdateShoppingCart", json);

                _manageData.SetStrategy(new UpdateData());
                await _manageData.GetDataAndDeserializeIt<object>($"Voucher/UpdateVoucherForSpecificUserWhenItIsUsed/{ActiveUser.UserId}/{CurrentSupermarket.Supermarket.SupermarketID}", "");

                await App.Current.MainPage.Navigation.PushAsync(new ShoppingCartView(Convert.ToDecimal(EarnedMoney)));

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
            await GetVoucherHistory();
        }

    }
}
