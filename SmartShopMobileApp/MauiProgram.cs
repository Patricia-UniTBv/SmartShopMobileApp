using Camera.MAUI;
using SmartShopMobileApp.Helpers;
using CommunityToolkit.Maui;
using Syncfusion.Maui.Core.Hosting;
using ZXing.Net.Maui.Controls;
using Microcharts.Maui;
using SmartShopMobileApp.Helpers.Okta;
using SmartShopMobileApp.Views;

namespace SmartShopMobileApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCameraView()
            .UseMauiCommunityToolkit()
            .UseMicrocharts()
            .ConfigureSyncfusionCore()
			.UseMauiMaps()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("MaterialIcons-Regular.ttf", "GoogleMaterialFont");
			})
            .UseBarcodeReader();
        builder.Services.AddSingleton<IHttpClientHandlerService, HttpClientHandlerService>();


        builder.Services.AddSingleton<LogInView>();

        var oktaClientConfiguration = new Helpers.Okta.OktaClientConfiguration()
        {
            // Use "https://{yourOktaDomain}/oauth2/default" for the "default" authorization server, or
            // "https://{yourOktaDomain}/oauth2/<MyCustomAuthorizationServerId>"

            OktaDomain = "https://{dev-41003006.okta.com}/oauth2/default",
            ClientId = "0oafi84r3eI4J6nXV5d7",
            RedirectUri = "http://localhost:80/",
            Browser = new WebBrowserAuthenticator()
        };

        builder.Services.AddSingleton(new OktaClient(oktaClientConfiguration));
        // 👆 new code
        return builder.Build();
	}
}
