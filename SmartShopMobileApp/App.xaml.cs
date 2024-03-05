using SmartShopMobileApp.Helpers.Okta;
using SmartShopMobileApp.Views;

namespace SmartShopMobileApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
        var oktaClientConfiguration = new Helpers.Okta.OktaClientConfiguration()
        {
            // Use "https://{yourOktaDomain}/oauth2/default" for the "default" authorization server, or
            // "https://{yourOktaDomain}/oauth2/<MyCustomAuthorizationServerId>"

            OktaDomain = "https://{dev-41003006.okta.com}/oauth2/default",
            ClientId = "0oafi84r3eI4J6nXV5d7",
            RedirectUri = "http://localhost:80/",
            Browser = new WebBrowserAuthenticator()
        };
        var oktaClient = new OktaClient(oktaClientConfiguration);
        MainPage = new NavigationPage(new LogInView(oktaClient));
    }
}
