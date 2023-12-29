using DTO;
using SmartShopMobileApp.ViewModels;
using System.Collections.ObjectModel;

namespace SmartShopMobileApp.Views;

public partial class HistoryAndStatisticsView : ContentPage
{
	public HistoryAndStatisticsView()
	{
		InitializeComponent();
		BindingContext = new HistoryAndStatisticsViewModel();
	}

	//public HistoryAndStatisticsView(ObservableCollection<ShoppingCartDTO> carts)
	//{
 //       InitializeComponent();
 //       BindingContext = new HistoryAndStatisticsViewModel( carts);
 //   }

   
}