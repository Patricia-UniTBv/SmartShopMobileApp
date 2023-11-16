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
    void CancelButton_Clicked(System.Object sender, System.EventArgs e)
    {
        CloseAsync();
    }
}