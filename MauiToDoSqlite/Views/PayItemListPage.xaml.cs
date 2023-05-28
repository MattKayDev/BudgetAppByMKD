using MauiBudgetApp.Data;
using MauiBudgetApp.Models;

namespace MauiBudgetApp.Views;

public partial class PayItemListPage : ContentPage
{

	private double incomeTotal = 0;
	private double expenseTotal = 0;

	public PayItemListPage()
	{
		InitializeComponent();
		UpdateTotals();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		PayItemItemDatabase database = await PayItemItemDatabase.Instance;
		expenseListView.ItemsSource = await database.GetExpenseItems();
		incomeListView.ItemsSource = await database.GetItemsAsync();
	}

	async void OnItemAdded(object sender, EventArgs eventArgs)
	{
		await OpenNewItemPage(await DisplayAlert("", "What would you like to add?", "Income", "Expense"));		
	}

	async void OnIncomeItemSelected(object sender, SelectedItemChangedEventArgs e)
	{
        if (e.SelectedItem != null)
        {
            await Navigation.PushAsync(new PayItemPage
            {
                BindingContext = e.SelectedItem as TodoItem
            });
        }
    }


    async void OnExpenseItemSelected(object sender, SelectedItemChangedEventArgs e)
	{
		if (e.SelectedItem != null)
		{
			await Navigation.PushAsync(new PayItemPage
			{
				BindingContext = e.SelectedItem as TodoItem
			});
		}
	}

	async Task OpenNewItemPage(bool isExpense)
	{
		if (isExpense)
		{
            await Navigation.PushAsync(new PayItemPage
            {
                BindingContext = new PayItem(isExpense)
            });
        }
		else
		{
            await Navigation.PushAsync(new PayItemPage
            {
                BindingContext = new PayItem(isExpense)
            });
        }
    }

	private async void UpdateTotals()
	{
        incomeTotal = await GetTotalsFor(true);
        expenseTotal = await GetTotalsFor(false);
        lblInitialText.Text = "Based on your income and expenses you are using";
        if (incomeTotal > 0)
        {
            double oneProcent = incomeTotal / 100;
            progressMoneyLeft.Progress = Math.Round(expenseTotal / oneProcent, 0);
			lblMoneyLeft.Text = $"{expenseTotal}/{incomeTotal}";
        }
		else
		{
			lblMoneyLeft.Text = $"{expenseTotal}/0";
			progressMoneyLeft.Progress = 100;
		}
    }

	private async Task<double> GetTotalsFor(bool isIncome)
	{
        PayItemItemDatabase database = await PayItemItemDatabase.Instance;
		if (isIncome)
		{
			var items = await database.GetIncomeItems();
            double total = 0;
            foreach (var item in items)
            {
                total += item.Amount;
            }

            return total;
        }
		else
		{
            var items = await database.GetExpenseItems();
            double total = 0;
            foreach (var item in items)
            {
                total += item.Amount;
            }

            return total;
        }

		
    }
}