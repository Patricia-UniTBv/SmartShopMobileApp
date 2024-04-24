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

namespace SmartShopMobileApp.ViewModels
{
    public partial class ProfileViewModel: ObservableObject 
    {
        [ObservableProperty]
        private UserDTO _currentUser;

        [ObservableProperty] 
        private ObservableCollection<string> _languageOptions;

        [ObservableProperty]
        private ObservableCollection<string> _currencyOptions;

        [ObservableProperty]
        private AuthResponseDTO _loggedUser;

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

        [Obsolete]
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged("SelectedLanguage");

                if (_selectedLanguage == "English")
                {
                    Task.Run(async () => await ChangeLanguage("en"));
                }
                else if (_selectedLanguage == "Romanian")
                {
                    Task.Run(async () => await ChangeLanguage("ro"));
                }
                else if (_selectedLanguage == "French")
                {
                    Task.Run(async () => await ChangeLanguage("fr"));
                }
            }
        }

        private string _selectedCurrency;
        [Obsolete]
        public string SelectedCurrency
        {
            get => _selectedCurrency;
            set
            {
                _selectedCurrency = value;
                OnPropertyChanged("SelectedCurrency");

                Task.Run(async () => await ChangeCurrency(_selectedCurrency));
            }
        }
        private readonly CurrencyConversionService _conversionService = new CurrencyConversionService();

        public ProfileViewModel()
        {
            var currentCulture = CultureInfo.CurrentCulture;
            var rm = new ResourceManager("SmartShopMobileApp.Resources.Languages.AppResources", typeof(AppResources).Assembly);
            var resourceSet = rm.GetResourceSet(currentCulture, true, true);

            _manageData = new ManageData();

            _authService = new AuthService();

            if (AuthenticationResultHelper.ActiveUser == null)
            {
                AuthenticationResultHelper.ActiveUser = new UserDTO();
            }

            AuthenticationResultHelper.ActiveUser.UserID = 1;
            //PROVIZORIU
            CurrentUser = AuthenticationResultHelper.ActiveUser;
            CurrentUser.FirstName = "Test";
            CurrentUser.LastName = "Testez";

            LanguageOptions = new ObservableCollection<string>()
            {
               "English",
               "Romanian",
               "French"
            };
            SelectedLanguage = currentCulture.DisplayName;

            CurrencyOptions = new ObservableCollection<string>()
            {
               "RON",
               "EUR",
               "USD",
               "GBP"
            };

        }

        [Obsolete]
        private async Task ChangeLanguage(string selectedLanguage)
        {
            try
            {

                _manageData.SetStrategy(new CreateData());
                var json = JsonConvert.SerializeObject(CurrentUser);

                await _manageData.GetDataAndDeserializeIt<UserDTO>($"User/UpdateLanguage?userId={CurrentUser.UserID}&language={selectedLanguage}", json);

                var language = new CultureInfo(selectedLanguage);
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(selectedLanguage);
                AppResources.Culture = language;

                Device.BeginInvokeOnMainThread(() =>
                {
                    App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new HomeView()));
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [Obsolete]
        private async Task ChangeCurrency(string selectedCurrency)
        {
            try
            {

                _manageData.SetStrategy(new CreateData());
                var json = JsonConvert.SerializeObject(CurrentUser);

                await _manageData.GetDataAndDeserializeIt<UserDTO>($"User/UpdateCurrency?userId={CurrentUser.UserID}&currency={selectedCurrency}", json);

                Device.BeginInvokeOnMainThread(() =>
                {
                    App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new ProfileView()));
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
            LoggedUser = null;
            App.NewLoggedUser = true;
            _authService.Logout();
            await Shell.Current.GoToAsync("//LogInView");
        }
    }
}
