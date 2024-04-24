using SmartShopMobileApp.Services;
using SmartShopMobileApp.Services.Interfaces;
using SmartShopMobileApp.Views;

namespace SmartShopMobileApp;

public partial class MainPage : ContentPage
{
    private readonly IAuthService _authService;
    public MainPage(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (await _authService.IsUserAuthenticated())
        {
            await Shell.Current.GoToAsync($"//{nameof(HomeView)}");
            //await Shell.Current.GoToAsync($"//{nameof(LogInView)}");
        }
        else
        {
            await Shell.Current.GoToAsync($"//{nameof(LogInView)}");
        }
    }
}

