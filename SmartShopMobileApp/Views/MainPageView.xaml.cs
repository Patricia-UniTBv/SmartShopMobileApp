using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.Resources.Languages;
using SmartShopMobileApp.Services.Interfaces;
using SmartShopMobileApp.ViewModels;
using System.Globalization;

namespace SmartShopMobileApp.Views;

public partial class MainPageView : ContentPage
{
    public LocalizationResourceManager LocalizationResourceManager
       => LocalizationResourceManager.Instance;

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
    public MainPageView(IAuthService authService)
    {
        InitializeComponent();
        _manageData = new ManageData();

        _authService = authService;

        BindingContext = new MainPageViewModel();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (await _authService.IsUserAuthenticated())
        {
            var activeUser = await _authService.GetAuthenticatedUserAsync();

            _manageData.SetStrategy(new GetData());
            var result = await _manageData.GetDataAndDeserializeIt<Tuple<string, string>>($"User/GetPreferredLanguageAndCurrency?userId={activeUser.UserId}", "");

            var switchToCulture = new CultureInfo(result.Item1);
            LocalizationResourceManager.Instance.SetCulture(switchToCulture);

            await Shell.Current.GoToAsync("//AppView");
        }
    }

}