using SmartShopMobileApp.ViewModels;

namespace SmartShopMobileApp.Views;

public partial class ShoppingCartView : ContentPage
{
	public ShoppingCartView(double discount)
	{
		InitializeComponent();
        ShoppingCartViewModel viewModel = new ShoppingCartViewModel();
        viewModel.VoucherDiscount = discount;
        BindingContext = viewModel;
    }

    public ShoppingCartView()
    {
        InitializeComponent();
        BindingContext = new ShoppingCartViewModel();
    }
}