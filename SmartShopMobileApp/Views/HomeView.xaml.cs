using DTO;
using SmartShopMobileApp.ViewModels;

namespace SmartShopMobileApp.Views;

public partial class HomeView : ContentPage
{
    private HomeViewModel homeVM;
	public HomeView()
    {
		InitializeComponent();
        homeVM= new HomeViewModel();
        BindingContext = new HomeViewModel();
    }

    public HomeView(List<SupermarketDTO> supermarkets)
    {
        InitializeComponent();
        homeVM = new HomeViewModel();
        homeVM.Supermarkets = supermarkets;
        BindingContext = new HomeViewModel();
    }

    //private async void AddButtonClicked(object sender, EventArgs e)
    //{
    //    string result = await DisplayPromptAsync("Add supermarket", "What's its name?");
    //    if (result != null)
    //    {
    //        homeVM.SupermarketInputName = result;
    //    }
    //}

}