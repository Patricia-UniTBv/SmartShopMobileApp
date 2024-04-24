using DTO;
using Microcharts;
using Microcharts.Maui;
using SkiaSharp;
using SmartShopMobileApp.Helpers;
using System.Collections.ObjectModel;

namespace SmartShopMobileApp.Views;

public partial class MonthlySpendingsView : ContentPage
{
    private List<ShoppingCartDTO> shoppingCarts;
    private Chart chart;
    private IManageData _manageData;
    public IManageData ManageData
    {
        get { return _manageData; }
        set { _manageData = value; }
    }
    private List<string> months = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

    public MonthlySpendingsView()
    {
        InitializeComponent();
        _manageData = new ManageData();

        //if (AuthenticationResultHelper.ActiveUser == null)
        //{
        //    AuthenticationResultHelper.ActiveUser = new UserDTO();
        //}

        //AuthenticationResultHelper.ActiveUser.UserID = 1;

        foreach (string month in months)
        {
            monthPicker.Items.Add(month);
        }

        for (int year = 2000; year <= DateTime.Now.Year; year++) 
        {
            yearPicker.Items.Add(year.ToString());
        }

        GetShoppingCarts();

        var currentDate = DateTime.Now;
        GetCategoryStatistics(currentDate.Month.ToString(),currentDate.Year);
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowChart();
    }

    private async Task ShowChart()
    {
        string selectedMonth = monthPicker.SelectedItem as string;
        int selectedYear = int.Parse(yearPicker.SelectedItem as string);

        await GetCategoryStatistics(selectedMonth, selectedYear);
    }

    //private async void MonthPicker_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string selectedMonth = monthPicker.SelectedItem as string;

    //    await GetCategoryStatistics(selectedMonth);
    //}

    private async Task GetShoppingCarts()
    {
        try
        {
            _manageData.SetStrategy(new GetData());

            shoppingCarts = await _manageData.GetDataAndDeserializeIt<List<ShoppingCartDTO>>($"ShoppingCart/GetAllTransactedShoppingCartsWithSupermarketByUserId?id={AuthenticationResultHelper.ActiveUser.UserId}", "");

            var lastFourMonths = DateTime.Now.AddMonths(-3); 

            var monthlySpendings = shoppingCarts
                .Where(cart => cart.CreationDate >= lastFourMonths) 
                .GroupBy(cart => new DateTime(cart.CreationDate.Year, cart.CreationDate.Month, 1))
                .Select(group => new
                {
                    Month = group.Key,
                    TotalAmount = group.Sum(cart => cart.TotalAmount)
                })
                .OrderBy(data => data.Month)
                .ToList();

            var entries = monthlySpendings
                .Select(data =>
                    new ChartEntry((float)data.TotalAmount)
                    {
                        Label = data.Month.ToString("MMM yyyy"),
                        ValueLabel = $"{data.TotalAmount} lei"
                    })
                .ToArray();


            chartView.Chart = new LineChart
            {
                Entries = entries,
                LineSize = 6,
                BackgroundColor = SKColor.Parse("#FFFFFF"),
                LabelTextSize = 30
            };

            for (int i = 0; i < entries.Length; i++)
            {
                entries[i].Color = SKColor.Parse(GetRandomColor()); 
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task GetCategoryStatistics(string selectedMonth, int selectedYear)
    {
        _manageData.SetStrategy(new GetData());

        var currentDate = DateTime.Now;
        var firstDayOfMonth = new DateTime(selectedYear, months.IndexOf(selectedMonth) + 1, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        var allShoppingCarts = await _manageData.GetDataAndDeserializeIt<List<ShoppingCartDTO>>($"ShoppingCart/GetAllTransactedShoppingCartsWithSupermarketByUserId?id={AuthenticationResultHelper.ActiveUser.UserId}", "");

        var shoppingCartsThisMonth = allShoppingCarts
            .Where(cart => cart.CreationDate >= firstDayOfMonth && cart.CreationDate <= lastDayOfMonth).ToList();

        _manageData.SetStrategy(new GetData());
        foreach (var cart in shoppingCartsThisMonth)
        {
            cart.CartItemsAsProducts = await _manageData.GetDataAndDeserializeIt<List<ProductDTO>>($"CartItem/GetItemsForShoppingCart?shoppingCartId={cart.ShoppingCartID}", "");        
        }

        _manageData.SetStrategy(new GetData());
        var categoryNames = await _manageData.GetDataAndDeserializeIt<List<CategoryDTO>>($"Category/GetAllCategories", "");

        var spendingByCategory = shoppingCartsThisMonth
            .SelectMany(cart => cart.CartItemsAsProducts) 
            .GroupBy(item => item.CategoryID)
            .Select(group => new
            {
                CategoryID = group.Key,
                TotalAmount = group.Sum(item => item.Price),
                CategoryName = categoryNames.FirstOrDefault(cat => cat.CategoryID == group.Key)?.Name
            })
            .ToList();

        var entries = spendingByCategory
            .Select(data =>
                new ChartEntry((float)data.TotalAmount)
                {
                    Label = $"{data.CategoryName}", 
                    ValueLabel = $"{data.TotalAmount} lei"
                })
            .ToArray();

        for (int i = 0; i < entries.Length; i++)
        {
            entries[i].Color = SKColor.Parse(GetRandomColor());
        }

        var donutChart = new DonutChart
        {
            Entries = entries,
            BackgroundColor = SKColor.Parse("#FFFFFF"),
            LabelTextSize = 28
        };

        chartView1.Chart = donutChart;

    }

    string GetRandomColor()
    {
        Random rand = new Random();
        return String.Format("#{0:X6}", rand.Next(0x1000000));
    }
}