using MauiBudgetApp.Data;
using MauiBudgetApp.Models;
using MauiBudgetApp.Services;
using System.ComponentModel;

namespace MauiBudgetApp.Views;

public partial class PayItemListPage : ContentPage
{
    PayItemService payItemService;
	private decimal incomeTotal = 0;
	private decimal expenseTotal = 0;

	public PayItemListPage()
	{
		InitializeComponent();
        this.payItemService = new PayItemService();
    }

	protected override async void OnAppearing()
	{
		base.OnAppearing();

        var expenseItems = await this.payItemService.GetExpenseItemsAsync();
        if (expenseItems.Count > 0)
        {
            expenseListView.ItemsSource = expenseItems;
            this.expenseTotal = this.payItemService.GetTotalFor(expenseItems);
            txtTotalExpense.Text = $"£ {this.expenseTotal}";
        }

        var incomeItems = await this.payItemService.GetIncomeItemsAsync();
        if (incomeItems.Count > 0)
        {
            incomeListView.ItemsSource = incomeItems;
            this.incomeTotal = this.payItemService.GetTotalFor(incomeItems);
            txtTotalIncome.Text = $"£ {this.incomeTotal}";            
        }    

        UpdateTotals(expenseItems.Count, incomeItems.Count);
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

	private void UpdateTotals(int expenseCount, int incomeCount)
	{
        if (expenseCount > 0 && incomeCount > 0)
        {
            decimal moneyLeftAmount = this.incomeTotal - this.expenseTotal;
            string moneyLeftText = $"£{moneyLeftAmount}";

            if (this.incomeTotal > 0)
            {
                decimal oneProcent = this.incomeTotal / 100;
                var progress = Convert.ToDouble((this.expenseTotal / oneProcent)) * 0.01;
                progressMoneyLeft.Progress = 1 - progress;

                lblMoneyOutOf.Text = $"{txtTotalExpense.Text} / {txtTotalIncome.Text}";

                if (this.expenseTotal > this.incomeTotal)
                {
                    lblDashboardStart.Text = "Based on your income and expenses you are using ";
                    lblDashboardAmount.Text = $"£{moneyLeftAmount * -1}";
                    lblDashboardEnd.Text = $" over your budget of £{this.incomeTotal}";
                    lblInitialText.Text = $"{lblDashboardStart.Text}{lblDashboardAmount.Text}{lblDashboardEnd.Text}";

                    lblMoneyLeftStart.Text = "You have ";
                    lblMoneyLeftAmount.Text = moneyLeftText;
                    lblMoneyLeftEnd.Text = " left of your budget.";
                }
                else
                {

                    lblDashboardStart.Text = "Based on your income and expenses you are using ";
                    lblDashboardAmount.Text = txtTotalExpense.Text;
                    lblDashboardEnd.Text = $" of your budget.";
                    lblInitialText.Text = $"{lblDashboardStart.Text}{txtTotalExpense.Text}{lblDashboardEnd.Text}";

                    lblMoneyLeftStart.Text = "You have ";
                    lblMoneyLeftAmount.Text = moneyLeftText;
                    lblMoneyLeftEnd.Text = " left of your budget.";
                }
            }
            else
            {
                lblMoneyOutOf.Text = $"£{this.expenseTotal} / £0";
                progressMoneyLeft.Progress = 0;
                lblMoneyLeft.Text = $"You are using £{this.expenseTotal} over your budget";
            }
        }
    }

    private async void OnSettingsClick(object sender, EventArgs e)
    {
        await DisplayAlert("Settings", "Nothing happens here yet!", "OK");
    }
}