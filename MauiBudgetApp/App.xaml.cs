using MauiBudgetApp.Services;
using MauiBudgetApp.Views;

namespace MauiBudgetApp;

public partial class App : Application
{
	public App(ExpensePayItemService expensePayItemService, IncomePayItemService incomePayItemService)
	{
		InitializeComponent();

		MainPage = new NavigationPage(new PayItemListPage(expensePayItemService, incomePayItemService))
		{
			BarTextColor = Color.FromRgb(255,255,255),
			BarBackgroundColor = Color.FromArgb("#3366ff")
		};
	}
}
