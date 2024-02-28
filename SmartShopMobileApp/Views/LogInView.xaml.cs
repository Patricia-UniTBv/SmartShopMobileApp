using SmartShopMobileApp.ViewModels;

namespace SmartShopMobileApp.Views;

public partial class LogInView : ContentPage
{
	public LogInView()
	{
		InitializeComponent();
		BindingContext = new LogInViewModel();
	}
}