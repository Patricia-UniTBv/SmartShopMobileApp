using DTO;
using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.Resources.Languages;
using SmartShopMobileApp.Services.Interfaces;
using SmartShopMobileApp.Views;
using System.Globalization;

namespace SmartShopMobileApp;

public partial class App : Application
{
    public static bool NewLoggedUser { get; set; }

    public App()
	{
		InitializeComponent();

        NewLoggedUser = false;

        MainPage = new AppShell();
    }

}
