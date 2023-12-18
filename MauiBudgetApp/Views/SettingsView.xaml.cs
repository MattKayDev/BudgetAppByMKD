using MauiBudgetApp.Models;
using MauiBudgetApp.Services;
using System.Globalization;

namespace MauiBudgetApp.Views;

public partial class SettingsView : ContentPage
{
    private ExpensePayItemService expenseService;
    private IncomePayItemService incomeService;

    public SettingsView(ExpensePayItemService expenseService, IncomePayItemService incomeService)
	{
		InitializeComponent();
        string cultureName = "en-GB";
        var culture = new CultureInfo(cultureName);

        this.expenseService = expenseService;
        this.incomeService = incomeService;
    }

    private async void OnSave_Clicked(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private async void btnClearSettings_Clicked(object sender, EventArgs e)
    {
        //bool res = await DisplayAlert("Warning!!!", "Settings will be cleared. Are you sure?", "Yes", "No");
        //if (res)
        //{
        //    if (await DisplayAlert("Please confirm", "Are you sure?", "Yes, Clear settings", "No"))
        //    {
        //        Preferences.Clear();
        //        await DisplayAlert("Cleared", "Settings have been cleared", "OK");
        //        await Navigation.PopAsync();
        //    }
        //}
    }

    private async void btnClearPaidItems_Clicked(object sender, EventArgs e)
    {
        bool cleared = false;

        var expenseItems = await this.expenseService.GetExpenseItemsAsync();
        if (expenseItems != null)
        {
            var itemsToClear = expenseItems.Where(e => e.IsPaid).ToList();
            var toClearCount = itemsToClear.Count();

            foreach (var item in itemsToClear)
            {
                if (await this.ClearPaidOnItem(item))
                {
                    toClearCount--;
                }
            }

            if (toClearCount == 0)
            {
                cleared = true;
            }

        }
        else
        {
            //display error getting expense items.
        }


        if (cleared)
        {
            await this.DisplayAlert("Cleared!", "All expense items have been cleared.", "OK");
        }
        else
        {
            // not cleared...
        }
    }

    private async Task<bool> ClearPaidOnItem(ExpenseItem item)
    {
        item.IsPaid = false;
        var updated = await this.expenseService.SaveItemAsync(item);

        return updated;
    }

    private async Task ShouldShowItemValues(bool showValues)
    {
        return;
    }
}