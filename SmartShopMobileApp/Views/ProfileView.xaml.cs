using SmartShopMobileApp.ViewModels;

namespace SmartShopMobileApp.Views;

public partial class ProfileView : ContentPage
{
	public ProfileView()
	{
		InitializeComponent();
		BindingContext = new ProfileViewModel();
	}
}