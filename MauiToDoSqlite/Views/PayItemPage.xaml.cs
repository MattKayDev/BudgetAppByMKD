using MauiBudgetApp.Models;
using MauiBudgetApp.Data;

namespace MauiBudgetApp.Views;

public partial class PayItemPage : ContentPage
{
	private bool updated = false;
    public PayItemPage()
	{
		InitializeComponent();

        txtName.TextChanged += TxtName_TextChanged;
        txtAmount.TextChanged += TxtAmount_TextChanged;
        switchIncome.HandlerChanged += SwitchIncome_HandlerChanged;
        switchPaid.HandlerChanged += SwitchPaid_HandlerChanged;
	}

    private void SwitchPaid_HandlerChanged(object sender, EventArgs e)
    {
        HandleUpdate();
    }

    private void SwitchIncome_HandlerChanged(object sender, EventArgs e)
    {
        HandleUpdate();
    }

    private void TxtAmount_TextChanged(object sender, TextChangedEventArgs e)
    {
        HandleUpdate();
    }

    private void TxtName_TextChanged(object sender, TextChangedEventArgs e)
    {
        HandleUpdate();
    }

    async void OnSaveClicked(object sender, EventArgs e)
	{
		var payItem = (PayItem)BindingContext;
		PayItemItemDatabase database = await PayItemItemDatabase.Instance;
		await database.SaveItemAsync(payItem);
		await Navigation.PopAsync();
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
        if (string.IsNullOrWhiteSpace(txtName.Text))
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