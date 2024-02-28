using SmartShopMobileApp.Views;

namespace SmartShopMobileApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        MainPage = new NavigationPage(new LogInView());
    }
}
