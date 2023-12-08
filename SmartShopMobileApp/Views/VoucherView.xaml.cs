using SmartShopMobileApp.ViewModels;

namespace SmartShopMobileApp.Views;

public partial class VoucherView : ContentPage
{
	public VoucherView()
	{
		InitializeComponent();
		BindingContext = new VoucherViewModel();
	}
}