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

        MainPage = new NavigationPage(new LogInView());
    }

}
