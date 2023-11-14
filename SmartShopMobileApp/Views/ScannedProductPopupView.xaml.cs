using CommunityToolkit.Maui.Views;
using SmartShopMobileApp.ViewModels;

namespace SmartShopMobileApp.Views;

public partial class ScannedProductPopupView : Popup
{
	public ScannedProductPopupView(string barcode)
	{
		InitializeComponent();
        BindingContext = new ScannedProductPopupViewModel(barcode);

    }
}