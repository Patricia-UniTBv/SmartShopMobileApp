using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.Resources.Languages;
using SmartShopMobileApp.Services;
using SmartShopMobileApp.Services.Interfaces;
using System.Globalization;

namespace SmartShopMobileApp;

public partial class AppShell : Shell
{


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

    public AppShell()
	{
		InitializeComponent();
        _manageData = new ManageData();
        _authService = new AuthService();

        SetAppCulture();
    }

    private async void SetAppCulture()
    {
        var activeUser = await _authService.GetAuthenticatedUserAsync();

        _manageData.SetStrategy(new GetData());
        var result = await _manageData.GetDataAndDeserializeIt<Tuple<string, string>>($"User/GetPreferredLanguageAndCurrency?userId={activeUser.UserId}", "");

        var language = new CultureInfo(result.Item1);
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(result.Item1);
        AppResources.Culture = language;
    }
}
