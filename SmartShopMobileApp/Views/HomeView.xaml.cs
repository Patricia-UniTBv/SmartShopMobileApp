using Android.Service.Autofill;
using SmartShopMobileApp.ViewModels;

namespace SmartShopMobileApp.Views;

public partial class HomeView : ContentPage
{
	public HomeView()
    {
		InitializeComponent();
        BindingContext = new HomeViewModel();
    }
}