using DTO;
using SmartShopMobileApp.Resources.Languages;
using SmartShopMobileApp.Views;
using System.Globalization;

namespace SmartShopMobileApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
    }

    private static void SetCultureAndNavigate()
    {
        var user = Authentic
        var language = new CultureInfo(user.PreferredLanguage);
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(user.PreferredLanguage);
        AppResources.Culture = language;
    }
}
