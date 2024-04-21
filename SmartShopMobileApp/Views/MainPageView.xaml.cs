using SmartShopMobileApp.Services;
using SmartShopMobileApp.ViewModels;

namespace SmartShopMobileApp.Views;

public partial class MainPageView : ContentPage
{
    private readonly IAuthService _authService;
    public MainPageView(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
        BindingContext = new MainPageViewModel();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (await _authService.IsUserAuthenticated())
        {
            await Shell.Current.GoToAsync("//AppView");
        }
    }
}