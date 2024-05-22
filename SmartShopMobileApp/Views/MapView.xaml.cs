using DTO;
using Microcharts;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using SmartShopMobileApp.Helpers;

namespace SmartShopMobileApp.Views;


public partial class MapView : ContentPage
{
    private IManageData _manageData;
    public IManageData ManageData
    {
        get { return _manageData; }
        set { _manageData = value; }
    }

    private int _supermarketId { get; set; }
    public MapView(int supermarketId)
    {
        InitializeComponent();

        _supermarketId = supermarketId;

        GetPinsForSupermarket();

    }

    public async void GetPinsForSupermarket()
    {
        _manageData = new ManageData();

        var stackLayout = new StackLayout();

        var map = new Microsoft.Maui.Controls.Maps.Map();
        map.MapType = MapType.Street;

        _manageData.SetStrategy(new GetData());
        var supermarketLocations = await _manageData.GetDataAndDeserializeIt<List<LocationDTO>>($"Location/GetLocationsBySupermarketId?supermarketId={_supermarketId}", "");
        bool appear = false;

        foreach (var location in supermarketLocations)
        {
            var supermarket = await _manageData.GetDataAndDeserializeIt<SupermarketDTO>($"Supermarket/GetSupermarketById?supermarketId={_supermarketId}", "");

            var pin = new Pin
            {
                Type = PinType.Place,
                Label = location.Address,
                Address = supermarket.Name,
                Location = new Location((double)location.Latitude, (double)location.Longidute)
            };

            pin.MarkerClicked += async (sender, e) =>
            {
                if (appear == false)
                {
                    var button = new Button
                    {
                        Text = "Start shopping",
                        BackgroundColor = Color.FromArgb("#512BD4"),
                        TextColor = Color.FromArgb("#FFFFFF"),
                        FontSize = 16,
                        CornerRadius = 5,
                        Padding = new Thickness(10),
                    };

                    button.Clicked += async (s, ev) =>
                    {
                        await Navigation.PushAsync(new BarcodeScannerView());
                    };

                    stackLayout.Children.Add(button);
                    appear = true;
                }
                
            };
            map.Pins.Add(pin);
        }

        stackLayout.Children.Add(map);

        map.MapType = MapType.Street;
        map.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(45.9432, 24.9668), Distance.FromKilometers(200)));
        Content = stackLayout;
    }
}