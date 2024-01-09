using Camera.MAUI;
using SmartShopMobileApp.Helpers;
using CommunityToolkit.Maui;
using Syncfusion.Maui.Core.Hosting;
using ZXing.Net.Maui.Controls;

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
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("MaterialIcons-Regular.ttf", "GoogleMaterialFont");
			})
            .UseBarcodeReader();
        builder.Services.AddSingleton<IHttpClientHandlerService, HttpClientHandlerService>();
        return builder.Build();
	}
}
