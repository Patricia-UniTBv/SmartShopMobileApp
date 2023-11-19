using Camera.MAUI;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using DTO;
using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.ViewModels;
using SmartShopMobileApp.Views;
using ZXing.QrCode.Internal;

namespace SmartShopMobileApp.Views;

public partial class BarcodeScannerView : ContentPage
{
    public BarcodeScannerView()
    {
        InitializeComponent();

        cameraView.BarCodeOptions = new Camera.MAUI.ZXingHelper.BarcodeDecodeOptions
        {
            PossibleFormats = { ZXing.BarcodeFormat.All_1D },
            TryHarder = true,
        };
    }
    public BarcodeScannerViewModel viewModel { get; set; }
    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.Cameras.Count > 0)
        {
            cameraView.Camera = cameraView.Cameras.First();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await cameraView.StopCameraAsync();
                await cameraView.StartCameraAsync();
            });
        }
    }

    private async void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            barcodeResult.Text = $"Barcode: {args.Result[0].Text}";
            BindingContext = new BarcodeScannerViewModel(args.Result[0].Text);
        });
    }

}