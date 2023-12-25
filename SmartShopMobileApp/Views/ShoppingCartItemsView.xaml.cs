using DTO;
using SmartShopMobileApp.ViewModels;

namespace SmartShopMobileApp.Views;

public partial class ShoppingCartItemsView : ContentPage
{
	public ShoppingCartItemsView(ShoppingCartDTO selectedCart)
	{
		InitializeComponent();
        ShoppingCartItemsViewModel viewModel = new ShoppingCartItemsViewModel(selectedCart);
        BindingContext = viewModel;
    }
}