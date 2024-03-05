using IdentityModel.OidcClient;
using SmartShopMobileApp.Helpers.Okta;
using SmartShopMobileApp.ViewModels;

namespace SmartShopMobileApp.Views;

public partial class LogInView : ContentPage
{
    private OktaClient _oktaClient;
    private LoginResult _authenticationData;

    public LogInView(OktaClient oktaClient)
	{
		InitializeComponent();
		//BindingContext = new LogInViewModel();
        _oktaClient = oktaClient;
    }

    public async void OnLoginClicked(object sender, EventArgs e)
    {
        var loginResult = await _oktaClient.LoginAsync();

        if (!loginResult.IsError)
        {
            _authenticationData = loginResult;
            LoginView.IsVisible = false;
            HomeView.IsVisible = true;

            UserInfoLvw.ItemsSource = loginResult.User.Claims;
            HelloLbl.Text = $"Hello, {loginResult.User.Claims.FirstOrDefault(x => x.Type == "name")?.Value}";
        }
        else
        {
            await DisplayAlert("Error", loginResult.ErrorDescription, "OK");
        }
    }

    public async void OnLogoutClicked(object sender, EventArgs e)
    {
        var logoutResult = await _oktaClient.LogoutAsync(_authenticationData.IdentityToken);

        if (!logoutResult.IsError)
        {
            _authenticationData = null;
            LoginView.IsVisible = true;
            HomeView.IsVisible = false;
        }
        else
        {
            await DisplayAlert("Error", logoutResult.ErrorDescription, "OK");
        }
    }
}