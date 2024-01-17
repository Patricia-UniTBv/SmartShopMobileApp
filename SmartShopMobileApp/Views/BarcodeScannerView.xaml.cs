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
        BindingContext = new BarcodeScannerViewModel();

        barcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
        {
            Formats = ZXing.Net.Maui.BarcodeFormat.Codabar | ZXing.Net.Maui.BarcodeFormat.Code39
                    | ZXing.Net.Maui.BarcodeFormat.Code93 | ZXing.Net.Maui.BarcodeFormat.Code128
                    | ZXing.Net.Maui.BarcodeFormat.Ean8 | ZXing.Net.Maui.BarcodeFormat.Ean13
                    | ZXing.Net.Maui.BarcodeFormat.Itf | ZXing.Net.Maui.BarcodeFormat.UpcE
                    | ZXing.Net.Maui.BarcodeFormat.UpcA,
            AutoRotate = true,
            Multiple = true
        };

    }

    private void barcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        var first = e.Results?.FirstOrDefault();

        if (first is null)
            return;

        Dispatcher.DispatchAsync(() =>
        {
            BindingContext = new BarcodeScannerViewModel(first.Value);
        });
    }

}