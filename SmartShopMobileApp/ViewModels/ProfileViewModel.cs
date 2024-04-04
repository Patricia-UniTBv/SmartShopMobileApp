using CommunityToolkit.Mvvm.ComponentModel;
using DTO;
using Newtonsoft.Json;
using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.Resources.Languages;
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

        private IManageData _manageData;
        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
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
            }
        }

        public ProfileViewModel()
        {
            var currentCulture = CultureInfo.CurrentCulture;
            var rm = new ResourceManager("SmartShopMobileApp.Resources.Languages.AppResources", typeof(AppResources).Assembly);
            var resourceSet = rm.GetResourceSet(currentCulture, true, true);

            _manageData = new ManageData();

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
               "Romanian"
            };
            SelectedLanguage = currentCulture.DisplayName;
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
                // App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new HomeView()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
