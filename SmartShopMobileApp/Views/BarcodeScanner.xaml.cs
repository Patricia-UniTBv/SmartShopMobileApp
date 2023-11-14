using Camera.MAUI;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using DTO;
using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.ViewModels;
using SmartShopMobileApp.Views;
using ZXing.QrCode.Internal;

namespace SmartShopMobileApp.Views;

public partial class BarcodeScanner : ContentPage
{
    public BarcodeScanner()
	{
		InitializeComponent();
        _manageData = new ManageData();
        cameraView.BarCodeOptions = new Camera.MAUI.ZXingHelper.BarcodeDecodeOptions
        {
            PossibleFormats = { ZXing.BarcodeFormat.All_1D }
        };
	}
    private IManageData _manageData;
    public IManageData ManageData
    {
        get { return _manageData; }
        set { _manageData = value; }
    }


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
        MainThread.BeginInvokeOnMainThread(() =>
        {
            barcodeResult.Text = $"Barcode: {args.Result[0].Text}";
            var popup = new ScannedProductPopupView(args.Result[0].Text);
            this.ShowPopupAsync(popup);
        });

       
       

    }
}