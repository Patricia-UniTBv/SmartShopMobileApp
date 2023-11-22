using SmartShopMobileApp.ViewModels;

namespace SmartShopMobileApp.Views;

public partial class ShoppingCartView : ContentPage
{
	public ShoppingCartView()
	{
		InitializeComponent();
		BindingContext = new ShoppingCartViewModel();
	}
}