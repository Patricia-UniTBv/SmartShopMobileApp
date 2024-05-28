using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using Newtonsoft.Json;
using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.Resources.Languages;
using SmartShopMobileApp.Services;
using SmartShopMobileApp.Services.Interfaces;
using SmartShopMobileApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using ZXing;

namespace SmartShopMobileApp.ViewModels
{
    public partial class ProfileViewModel: ObservableObject 
    {
        public ProfileViewModel()
        {
            _manageData = new ManageData();
            _authService = new AuthService();
            ActiveUser = AuthenticatedUser.ActiveUser;

            LanguageOptions = new ObservableCollection<string>()
            {
               "English",
               "Romanian",
               "French"
            };
            SelectedLanguage = ActiveUser.PreferredLanguage.ToUpper();

            CurrencyOptions = new ObservableCollection<string>()
            {
               "RON",
               "EUR",
               "USD",
               "GBP"
            };
            InitialCurrency = PreferredCurrency.Value;

        }

        [ObservableProperty]
        private string _initialCurrency;
        [ObservableProperty] 
        private ObservableCollection<string> _languageOptions;

        [ObservableProperty]
        private ObservableCollection<string> _currencyOptions;

        [ObservableProperty]
        private AuthResponseDTO _activeUser = new();

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

        private string _selectedLanguage;

        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged("SelectedLanguage");

                if (_selectedLanguage == "English")
                {
                    ChangeLanguage("en");
                }
                else if (_selectedLanguage == "Romanian")
                {
                    ChangeLanguage("ro");
                }
                else if (_selectedLanguage == "French")
                {
                    ChangeLanguage("fr");
                }
            }
        }

        private string _selectedCurrency;
        public string SelectedCurrency
        {
            get => _selectedCurrency;
            set
            {
                _selectedCurrency = value;
                OnPropertyChanged("SelectedCurrency");
                if (_selectedCurrency != InitialCurrency)
                { 
                    ChangeCurrency(_selectedCurrency);
                    InitialCurrency = SelectedCurrency;
                    OnPropertyChanged("SelectedCurrency");
                    OnPropertyChanged("InitialCurrency");
                }
            }
        }

        private async Task ChangeLanguage(string selectedLanguage)
        {
            try
            {
                _manageData.SetStrategy(new CreateData());

                await _manageData.GetDataAndDeserializeIt<UserDTO>($"User/UpdateLanguage?userId={ActiveUser.UserId}&language={selectedLanguage}", "");

                var language = new CultureInfo(selectedLanguage);
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(selectedLanguage);
                AppResources.Culture = language;
                LocalizationResourceManager.Instance.SetCulture(language);

                await App.Current.MainPage.Navigation.PushAsync(new HomeView());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private async Task ChangeCurrency(string selectedCurrency)
        {
            try
            {
                _manageData.SetStrategy(new CreateData());

                await _manageData.GetDataAndDeserializeIt<UserDTO>($"User/UpdateCurrency?userId={ActiveUser.UserId}&currency={selectedCurrency}", "");
                PreferredCurrency.Value = selectedCurrency;

                Device.BeginInvokeOnMainThread(() =>
                {
                    App.Current.MainPage.Navigation.PushAsync(new ProfileView());
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [RelayCommand]
        private async Task LogOut()
        {
            ActiveUser = null;
            App.NewLoggedUser = true;

            _authService.Logout();

            await Shell.Current.GoToAsync("//LogInView");
        }

    }
}
