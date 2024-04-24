using Camera.MAUI;
using SmartShopMobileApp.Helpers;
using CommunityToolkit.Maui;
using Syncfusion.Maui.Core.Hosting;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;
using Microcharts.Maui;
using SmartShopMobileApp.Services;
using SmartShopMobileApp.Views;
using SmartShopMobileApp.ViewModels;
using SmartShopMobileApp.Services.Interfaces;

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

        builder.Services.AddCustomApiHttpClient();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IHttpClientHandlerService, HttpClientHandlerService>();

#if DEBUG
		//builder.Logging.AddDebug();
#endif
		builder.Services.AddTransient<AuthService>();
		builder.Services.AddTransient<ProfileView>();

        builder.Services.AddTransient<MainPageView>();
        builder.Services.AddTransient<MainPageViewModel>();

        //builder.Services.AddTransient<HomeView>();
        //builder.Services.AddTransient<HomeViewModel>();

        builder.Services.AddTransient<LogInView>();
        builder.Services.AddTransient<LogInViewModel>();

        builder.Services.AddTransient<SignUpView>();
        builder.Services.AddTransient<SignUpViewModel>();

        return builder.Build();
	}
}
public static class MauiProgramExtensions
{
    public static IServiceCollection AddCustomApiHttpClient(this IServiceCollection services)
    {
        services.AddSingleton<IPlatformHttpMessageHandler>(_ =>
        {
#if ANDROID
            return new SmartShopMobileApp.Platforms.Android.AndroidHttpMessageHandler();

#endif
            return null;
        });

        services.AddHttpClient(AppConstants.HttpClientName, httpClient =>
        {
            var baseAddress =
                    DeviceInfo.Platform == DevicePlatform.Android
                        ? "https://10.0.2.2:7157"
                        : "https://localhost:7157";

            httpClient.BaseAddress = new Uri(baseAddress);
        })
            .ConfigureHttpMessageHandlerBuilder(builder =>
            {
                var platfromHttpMessageHandler = builder.Services.GetRequiredService<IPlatformHttpMessageHandler>();
                builder.PrimaryHandler = platfromHttpMessageHandler.GetHttpMessageHandler();
            });

        return services;
    }
}