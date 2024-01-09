﻿using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using Newtonsoft.Json;
using SmartShopMobileApp.HelperModels;
using SmartShopMobileApp.Helpers;
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
            VouchersHistory = new ObservableCollection<VoucherHistory>();
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
        public decimal? _earnedMoney;

        [ObservableProperty]
        public ObservableCollection<VoucherHistory> _vouchersHistory;

        private async Task GetEarnedMoney()
        {
            try
            {
                _manageData.SetStrategy(new GetData());
                var voucher = await _manageData.GetDataAndDeserializeIt<VoucherDTO>($"Voucher/GetVoucherForUserAndSupermarket/{AuthenticationResultHelper.ActiveUser.UserID}/{CurrentSupermarket.Supermarket.SupermarketID}", "");
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
                var allShoppingCarts = await _manageData.GetDataAndDeserializeIt<List<ShoppingCartDTO>>($"ShoppingCart/GetAllTransactedShoppingCartsByUserId?id={AuthenticationResultHelper.ActiveUser.UserID}", "");
                foreach (var cart in allShoppingCarts)
                {
                    if (cart.TotalAmount > 10) // aici trebuie sa modific cu 150 (10 este doar pentru test ca sa apara in lista cu istoricul)
                    {
                        var newVoucherHistory = new VoucherHistory();
                        newVoucherHistory.CartCreationDate = cart.CreationDate.ToShortDateString();
                        newVoucherHistory.CreationDate = cart.CreationDate;
                        newVoucherHistory.TotalAmount = cart.TotalAmount.ToString().Substring(0, 4);
                        newVoucherHistory.ValueModification = "+ " + (0.05m * cart.TotalAmount).ToString().Substring(0,4) + " lei";
                        newVoucherHistory.TextColor = Colors.Green;
                        VouchersHistory.Add(newVoucherHistory);
                    }
                 
                }

                var usedDiscounts = await _manageData.GetDataAndDeserializeIt<List<TransactionDTO>>($"Transaction/GetAllTransactionsWithDiscount/{AuthenticationResultHelper.ActiveUser.UserID}/{CurrentSupermarket.Supermarket.SupermarketID}", "");
                foreach (var transaction in usedDiscounts)
                {
                    if (transaction.VoucherDiscount != 0)
                    { 
                        var newVoucherHistory = new VoucherHistory();
                        newVoucherHistory.CartCreationDate = transaction.TransactionDate.ToShortDateString();
                        newVoucherHistory.CreationDate = transaction.TransactionDate;
                        newVoucherHistory.ValueModification = "- " + (transaction.VoucherDiscount).ToString() + " lei";
                        newVoucherHistory.TotalAmount = transaction.TotalAmount.ToString().Substring(0, 4);
                        newVoucherHistory.TextColor = Colors.Red;
                        VouchersHistory.Add(newVoucherHistory); 
                    }
                }

                var list = VouchersHistory.OrderByDescending(t => t.CreationDate.Month).OrderByDescending(t => t.CreationDate.Day).ToObservableCollection();
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
                var latestShoppingCart = await _manageData.GetDataAndDeserializeIt<ShoppingCartDTO>($"ShoppingCart/GetLatestShoppingCartByUserId?id={AuthenticationResultHelper.ActiveUser.UserID}", "");
                latestShoppingCart.TotalAmount -= Convert.ToDecimal(EarnedMoney);

                _manageData.SetStrategy(new UpdateData());
                var json = JsonConvert.SerializeObject(latestShoppingCart);
                await _manageData.GetDataAndDeserializeIt<object>($"ShoppingCart/UpdateShoppingCart", json);

                _manageData.SetStrategy(new UpdateData());
                await _manageData.GetDataAndDeserializeIt<object>($"Voucher/UpdateVoucherForSpecificUserWhenItIsUsed/{AuthenticationResultHelper.ActiveUser.UserID}/{CurrentSupermarket.Supermarket.SupermarketID}", "");

                await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new ShoppingCartView(Convert.ToDecimal(EarnedMoney))));

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
