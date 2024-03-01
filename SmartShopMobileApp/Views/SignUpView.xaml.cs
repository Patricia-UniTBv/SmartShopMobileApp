using SmartShopMobileApp.ViewModels;

namespace SmartShopMobileApp.Views;

public partial class SignUpView : ContentPage
{
	public SignUpView()
	{
		InitializeComponent();
		BindingContext = new SignUpViewModel();
	}
}