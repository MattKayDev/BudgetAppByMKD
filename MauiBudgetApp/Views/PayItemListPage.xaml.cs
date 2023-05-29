using MauiBudgetApp.Data;
using MauiBudgetApp.Models;
using System.ComponentModel;

namespace MauiBudgetApp.Views;

public partial class PayItemListPage : ContentPage
{

	private double incomeTotal = 0;
	private double expenseTotal = 0;
	private bool tablesExist;

	public PayItemListPage()
	{
		InitializeComponent();
    }

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		PayItemItemDatabase database = await PayItemItemDatabase.Instance;
		var items = await database.GetItemsAsync();
		if (items.Count > 0)
		{
            expenseListView.ItemsSource = await database.GetExpenseItems();
			incomeListView.ItemsSource = await database.GetIncomeItems();

            UpdateTotals();
        }
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
                BindingContext = e.SelectedItem as PayItem
            });
        }
    }


    async void OnExpenseItemSelected(object sender, SelectedItemChangedEventArgs e)
	{
		if (e.SelectedItem != null)
		{
			await Navigation.PushAsync(new PayItemPage
			{
				BindingContext = e.SelectedItem as PayItem
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
        double moneyLeftAmount = incomeTotal - expenseTotal;

        string expenseTotalText = $"£{String.Format("{0:.##}", expenseTotal)}";
        string incomeTotalText = $"£{String.Format("{0:.##}", incomeTotal)}";
        string moneyLeftText = $"£{String.Format("{0:.##}", moneyLeftAmount)}";

        txtTotalIncome.Text = incomeTotalText;
        txtTotalExpense.Text = expenseTotalText;

        //lblExpenseTotal.Text = expenseTotalText;

        //lblInitialText.Text = $"Based on your income and expenses you are using {String.Format("{0:.##}", expenseTotal)} of your budget.";

        if (incomeTotal > 0)
        {
            double oneProcent = incomeTotal / 100;
            var progress = (expenseTotal / oneProcent) * 0.01;
            progressMoneyLeft.Progress = 1 - progress;

			lblMoneyOutOf.Text = $"{expenseTotalText} / {incomeTotalText}";

            if (expenseTotal > incomeTotal)
            {
                lblDashboardStart.Text = "Based on your income and expenses you are using ";
                lblDashboardAmount.Text = $"£{moneyLeftAmount * -1}";
                lblDashboardEnd.Text = $" over your budget of £{incomeTotal}";
                //lblInitialText.Text = $"{lblDashboardStart.Text}{expenseTotalText}{lblDashboardEnd.Text}";
                lblInitialText.Text = $"{lblDashboardStart.Text}{lblDashboardAmount.Text}{lblDashboardEnd.Text}";

                lblMoneyLeftStart.Text = "You have ";
                lblMoneyLeftAmount.Text = moneyLeftText;
                lblMoneyLeftEnd.Text = " left of your budget.";
            }
            else
            {

                lblDashboardStart.Text = "Based on your income and expenses you are using ";
                lblDashboardAmount.Text = expenseTotalText;
                lblDashboardEnd.Text = $" of your budget.";
                lblInitialText.Text = $"{lblDashboardStart.Text}{expenseTotalText}{lblDashboardEnd.Text}";

                lblMoneyLeftStart.Text = "You have ";
                lblMoneyLeftAmount.Text = moneyLeftText;
                lblMoneyLeftEnd.Text = " left of your budget.";
                //lblMoneyLeft.Text = $"You have £{String.Format("{0:.##}", moneyLeft)} left of your budget";
            }
            
        }
		else
		{
			lblMoneyOutOf.Text = $"£{String.Format("{0:.##}", expenseTotal)} / £0";
			progressMoneyLeft.Progress = 0;
            lblMoneyLeft.Text = $"You are using £{String.Format("{0:.##}", expenseTotal)} over your budget";
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

    private async void OnSettingsClick(object sender, EventArgs e)
    {
        await DisplayAlert("Settings", "Nothing happens here yet!", "OK");
    }
}