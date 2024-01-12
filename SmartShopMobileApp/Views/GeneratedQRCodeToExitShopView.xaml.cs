using SmartShopMobileApp.ViewModels;

namespace SmartShopMobileApp.Views;

public partial class GeneratedQRCodeToExitShopView : ContentPage
{
	public GeneratedQRCodeToExitShopView()
	{
		InitializeComponent();
		BindingContext = new GeneratedQRCodeToExitShopViewModel();
	}
}