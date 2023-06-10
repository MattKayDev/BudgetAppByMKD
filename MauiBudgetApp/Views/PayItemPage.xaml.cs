using MauiBudgetApp.Models;
using MauiBudgetApp.Data;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace MauiBudgetApp.Views;

public partial class PayItemPage : ContentPage
{
	private bool updated = false;
    private bool isLeaving = false;
    public PayItemPage()
	{
		InitializeComponent();
        
    }

    async void OnSaveClicked(object sender, EventArgs e)
	{
		var payItem = (PayItem)BindingContext;
        if (payItem != null )
        {
            PayItemItemDatabase database = await PayItemItemDatabase.Instance;
            var res = await database.SaveItemAsync(payItem);
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("ERROR", "Error occured!", "OK");
            await Navigation.PopAsync();
        }
	}

	async void OnDeleteClicked(object sender, EventArgs e)
	{
        var payItem = (PayItem)BindingContext;
        if (await DisplayAlert("Are you sure?", $"Are you sure you want to delete {payItem.Name}?", "Yes", "No"))
        {
            PayItemItemDatabase database = await PayItemItemDatabase.Instance;
            await database.DeleteItemAsync(payItem);
            await Navigation.PopAsync();
        }
	}

    private void HandleUpdate()
    {
        if (!updated)
        {
            updated = true;
        }
    }
    async Task CancelItem()
    {
        try
        {
            var payItem = (PayItem)BindingContext;

            if (string.IsNullOrWhiteSpace(payItem.Name))
            {
                await Navigation.PopAsync();
            }

            if (updated)
            {
                bool update = await DisplayAlert("Hold on...", "You have updated some information for this item. Do you want to update or discard?", "Update", "Discard");
                if (update)
                {
                    //OnSaveClicked(sender, e);
                }
                else
                {
                    await Navigation.PopAsync();
                }
            }
            else
            {
                await Navigation.PopAsync();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("ERROR", ex.Message, "OK");
            await Navigation.PopAsync();
        }
    }

    private void txtName_TextChanged(object sender, TextChangedEventArgs e)
    {
        HandleUpdate();
    }

    public ICommand CancelCommand { get; private set; }

    private async Task OnCancelClicked(object sender, NavigatingFromEventArgs e)
    {
        //await CancelItem(sender, e);
    }
}