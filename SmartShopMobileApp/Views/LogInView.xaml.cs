using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.Services.Interfaces;
using SmartShopMobileApp.ViewModels;
using System.Globalization;

namespace SmartShopMobileApp.Views;

public partial class LogInView : ContentPage
{
	public LogInView(IAuthService authService)
    {
        InitializeComponent();
        _manageData = new ManageData();

        _authService = authService;

        BindingContext = new LogInViewModel(_authService);
    }
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

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (await _authService.IsUserAuthenticated())
        {
            SetCultureAndPreferrences();
            await Shell.Current.GoToAsync("//AppView");
        }
        await Shell.Current.GoToAsync("//AppView");
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
    }
}