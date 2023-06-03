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
        var incomeItems = await this.payItemService.GetIncomeItemsAsync();
        if (expenseItems.Count > 0)
        {
            expenseListView.ItemsSource = expenseItems;
            this.expenseTotal = this.payItemService.GetTotalFor(expenseItems);
            this.txtTotalExpense.IsVisible = true;
            txtTotalExpense.Text = $"£{this.expenseTotal}";

            var gotLeftToPay = this.payItemService.GetLeftToPay(expenseItems);
            if (gotLeftToPay > 0 && gotLeftToPay != this.expenseTotal)
            {
                txtExpensesPaid.Text = $"Left to pay £{gotLeftToPay} / ";
                txtExpensesPaid.IsVisible = true;
            }
            else
            {
                txtExpensesPaid.IsVisible = false;
            }
        }
        else
        {
            this.txtTotalExpense.IsVisible = false;
            this.expenseTotal = 0;
            expenseListView.ItemsSource = new List<PayItem>();
        }

        if (incomeItems.Count > 0)
        {
            incomeListView.ItemsSource = incomeItems;
            this.incomeTotal = this.payItemService.GetTotalFor(incomeItems);
            txtTotalIncome.IsVisible = true;
            txtTotalIncome.Text = $"£{this.incomeTotal}";            
        }
        else
        {
            txtTotalIncome.IsVisible = false;
            incomeListView.ItemsSource = new List<PayItem>();
        }

        UpdateTotals(expenseItems.Count, incomeItems.Count);
    }

	async void OnItemAdded(object sender, EventArgs eventArgs)
	{
        await AddItem();
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

    /// <summary>
    /// Opens a new item page 
    /// </summary>
    /// <param name="isExpense">Is the new item expense, defaults to True</param>
    /// <returns></returns>
	async Task OpenNewItemPage(bool isExpense = true)
	{
        await Navigation.PushAsync(new PayItemPage
        {
            BindingContext = new PayItem(isExpense)
        });
    }

	private void UpdateTotals(int expenseCount, int incomeCount)
	{
        this.HideMainDashboard(false);

        if (incomeCount > 0 && expenseCount == 0 )
        {
            this.SetupDashboardWithIncomeOnly();
        }

        if (expenseCount > 0 && incomeCount == 0 )
        {
            this.SetupDashboardWithExpenseOnly();
        }

        if (expenseCount > 0 && incomeCount > 0)
        {
            this.SetupDashboardMain();
        }

        if (incomeCount == 0 && expenseCount == 0)
        {
            this.HideMainDashboard(true);
        }
    }

    private async void OnSettingsClick(object sender, EventArgs e)
    {
        //await DisplayAlert("Settings", "Nothing happens here yet!", "OK");
        await Navigation.PushAsync(new SettingsView());
    }

    private void SetupDashboardWithIncomeOnly()
    {
        progressMoneyLeft.Progress = 1;
        lblMoneyOutOf.IsVisible = false;
        lblDashboardStart.Text = "It appears you have no expenses.";
        //lblDashboardAmount.Text = $"£{moneyLeftAmount * -1}";
        //lblDashboardEnd.Text = $" over your budget of £{this.incomeTotal}";
        lblInitialText.Text = $"{lblDashboardStart.Text}";

        lblMoneyLeftStart.Text = "You have ";
        lblMoneyLeftAmount.Text = $"£ {this.incomeTotal}";
        lblMoneyLeftEnd.Text = " left of your budget.";
    }

    private void SetupDashboardWithExpenseOnly()
    {
        progressMoneyLeft.Progress = 0;
        lblMoneyOutOf.IsVisible = false;
        lblDashboardStart.Text = "It appears you have no income, but you are using ";
        lblDashboardAmount.Text = $"£{this.expenseTotal}";
        lblDashboardEnd.Text = "";
        lblInitialText.Text = $"{lblDashboardStart.Text}{lblDashboardAmount.Text}{lblDashboardEnd.Text}";

        lblMoneyLeftStart.Text = "Apparently you are ";
        lblMoneyLeftAmount.Text = $"£{this.expenseTotal}";
        lblMoneyLeftEnd.Text = " out of budget.";
    }

    private void SetupDashboardMain()
    {
        decimal moneyLeftAmount = this.incomeTotal - this.expenseTotal;
        string moneyLeftText = $"£{moneyLeftAmount}";

        if (this.incomeTotal > 0)
        {
            decimal oneProcent = this.incomeTotal / 100;
            var progress = Convert.ToDouble((this.expenseTotal / oneProcent)) * 0.01;
            progressMoneyLeft.Progress = 1 - progress;


            lblMoneyOutOf.IsVisible = true;
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

    private void HideMainDashboard(bool hide)
    {
            gridDashboardMain.IsVisible = !hide;
            gridDashboardEmpty.IsVisible = hide;
    }

    private async Task AddItem()
    {
        var answer = await DisplayActionSheet("What would you like to add?", "Cancel", null, new string[] { "Income", "Expense" });
        switch (answer)
        {
            case "Income":
                {
                    await OpenNewItemPage(false);
                    break;
                }
            case "Expense":
                {
                    await OpenNewItemPage();
                    break;
                }
            default:
                break;
        }
    }

    private async void btnDashboardAddItem_Clicked(object sender, EventArgs e)
    {
        await AddItem();
    }
}