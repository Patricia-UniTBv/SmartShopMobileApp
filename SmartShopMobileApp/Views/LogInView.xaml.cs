using SmartShopMobileApp.Services;
using SmartShopMobileApp.ViewModels;

namespace SmartShopMobileApp.Views;

public partial class LogInView : ContentPage
{
	private readonly IAuthService _authService;
	public LogInView(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
        BindingContext = new LogInViewModel(_authService);
    }

}