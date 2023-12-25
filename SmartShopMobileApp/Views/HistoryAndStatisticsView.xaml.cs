using SmartShopMobileApp.ViewModels;

namespace SmartShopMobileApp.Views;

public partial class HistoryAndStatisticsView : ContentPage
{
	public HistoryAndStatisticsView()
	{
		InitializeComponent();
		BindingContext = new HistoryAndStatisticsViewModel();
	}

    private void OnShowFiltersClicked(object sender, EventArgs e)
    {
        FilterGrid.IsVisible = !FilterGrid.IsVisible;
    }
}