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
        var map = new Microsoft.Maui.Controls.Maps.Map();
        map.MapType = MapType.Street;

        _manageData.SetStrategy(new GetData());
        var supermarketLocations = await _manageData.GetDataAndDeserializeIt<List<LocationDTO>>($"Location/GetLocationsBySupermarketId?supermarketId={_supermarketId}", "");

        foreach (var location in supermarketLocations)
        {
            var pin = new Pin
            {
                Type = PinType.Place,
                Label = location.Address,
                Address = location.Address,
                Location = new Location((double)location.Latitude, (double)location.Longidute)
        };
            map.Pins.Add(pin);
        }

        Content = map;
    }
}