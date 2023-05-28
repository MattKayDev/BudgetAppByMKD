using MauiBudgetApp.Models;
using MauiBudgetApp.Data;

namespace MauiBudgetApp.Views;

public partial class PayItemPage : ContentPage
{
	private bool updated = false;
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
            await database.SaveItemAsync(payItem);
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
		PayItemItemDatabase database = await PayItemItemDatabase.Instance;
		await database.DeleteItemAsync(payItem);
		await Navigation.PopAsync();
	}


	async void OnCancelClicked(object sender, EventArgs e)
	{
        var payItem = (PayItem)BindingContext;

        if (string.IsNullOrWhiteSpace(payItem.Name))
        {
            await Navigation.PopAsync();
        }

		if (updated)
		{
			if (await DisplayAlert("Hold on...", "You have updated some information for this item. Do you want to update or discard?", "Update", "Discard"))
			{
				OnSaveClicked(sender, e);
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

    private void HandleUpdate()
    {
        if (!updated)
        {
            updated = true;
        }
    }

}