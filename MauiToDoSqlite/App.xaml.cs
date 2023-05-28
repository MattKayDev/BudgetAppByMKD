using MauiBudgetApp.Views;

namespace MauiBudgetApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new NavigationPage(new PayItemListPage())
		{
			BarTextColor = Color.FromRgb(255, 255, 255)
		};
	}
}
