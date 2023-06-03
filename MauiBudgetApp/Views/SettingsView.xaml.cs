namespace MauiBudgetApp.Views;

public partial class SettingsView : ContentPage
{
 
	public SettingsView()
	{
		InitializeComponent();
        int payDate = Preferences.Default.Get("PayDay", 1);
        txtDayOfTheMonth.Text = payDate.ToString();
    }

    private async void OnSave_Clicked(object sender, EventArgs e)
    {
        try
        {
            int numberOfDays = Convert.ToInt32(txtDayOfTheMonth.Text);
            if (numberOfDays > 30)
            {
                await DisplayAlert("Invalid day", "Day can not be bigger than 30", "OK");
                Preferences.Default.Set("PayDay", 30);
                txtDayOfTheMonth.Text = "30";
            }
            else if (numberOfDays < 1)
            {
                await DisplayAlert("Invalid day", "Day can not be smaller than 1", "OK");
                Preferences.Default.Set("PayDay", 1);
                txtDayOfTheMonth.Text = "1";
            }
            else
            {
                Preferences.Default.Set("PayDay", numberOfDays);
                await DisplayAlert("Updated!", "'Paid' day has been updated", "OK");
                await Navigation.PopAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private async void btnClearSettings_Clicked(object sender, EventArgs e)
    {
        bool res = await DisplayAlert("Warning!!!", "Settings will be cleared. Are you sure?", "Yes", "No");
        if (res)
        {
            if (await DisplayAlert("Please confirm", "Are you sure?", "Yes, Clear settings", "No"))
            {
                Preferences.Clear();
                await DisplayAlert("Cleared", "Settings have been cleared", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}