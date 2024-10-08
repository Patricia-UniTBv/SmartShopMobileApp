﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.Services.Interfaces;
using SmartShopMobileApp.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.ViewModels
{
    public partial class LogInViewModel: ObservableObject
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        private string _email;
        [ObservableProperty]
        private string _password;
        [ObservableProperty]
        private UserDTO _existingUser;
        public LogInViewModel(IAuthService authService)
        {
            _manageData = new ManageData();
            _authService = authService;
        }
        private IManageData _manageData;

        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
        }

        [RelayCommand]
        private async void LogIn()
        {
            _manageData.SetStrategy(new GetData());
            ExistingUser = null;
            var hashedPassword = HashPassword(Password);
            ExistingUser = await _manageData.GetDataAndDeserializeIt<UserDTO>($"User/GetUserByEmailAndPassword?email={Email}&password={hashedPassword}", "");

            var error = await _authService.LoginAsync(new LoginRequestDTO(ExistingUser.Email, ExistingUser.Password));
            if (string.IsNullOrWhiteSpace(error))
            {
                SetCultureAndPreferrences();
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", error, "Ok");
            }
        }

        private async void SetCultureAndPreferrences()
        {
            var activeUser = await _authService.GetAuthenticatedUserAsync();
            AuthenticatedUser.ActiveUser = activeUser;

            _manageData.SetStrategy(new GetData());

            var result = await _manageData.GetDataAndDeserializeIt<Tuple<string, string>>($"User/GetPreferredLanguageAndCurrency?userId={activeUser.UserId}", "");

            AuthenticatedUser.ActiveUser.PreferredLanguage = result.Item1;
            AuthenticatedUser.ActiveUser.PreferredCurrency = result.Item2;

            var switchToCulture = new CultureInfo(result.Item1);
            LocalizationResourceManager.Instance.SetCulture(switchToCulture);

            PreferredCurrency.Value = result.Item2;

            await Shell.Current.GoToAsync("//AppView/HomeView");
        }

        private static string HashPassword(string password)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        [RelayCommand]
        private async Task OpenSignUpPage()
        {
            await App.Current.MainPage.Navigation.PushAsync(new SignUpView());
        }
    }
}
