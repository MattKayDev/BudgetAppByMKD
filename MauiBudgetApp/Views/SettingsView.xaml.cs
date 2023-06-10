using System.Globalization;

namespace MauiBudgetApp.Views;

public partial class SettingsView : ContentPage
{
 
	public SettingsView()
	{
		InitializeComponent();
        string cultureName = "en-GB";
        var culture = new CultureInfo(cultureName);
        DateTime payDate = Preferences.Default.Get("PayDay", DateTime.Today);
        
        lblNextResetDate.Text = payDate.ToString("dd/MM/yyyy");
        txtDayOfTheMonth.Text = payDate.Day.ToString();
    }

    private async void OnSave_Clicked(object sender, EventArgs e)
    {
        try
        {
            int numberOfDays = Convert.ToInt32(txtDayOfTheMonth.Text);
            if (numberOfDays > 31)
            {
                await DisplayAlert("Invalid day", "Day can not be bigger than 31", "OK");

                var today = DateTime.Today;
                var firstDay = new DateTime(today.Year, today.Month, 1);
                var lastDay = firstDay.AddMonths(1).AddDays(-1);
                var day = lastDay.Day;
                Preferences.Default.Set("PayDay", lastDay);
                Preferences.Default.Set("ClearedPaid", false);
                txtDayOfTheMonth.Text = $"{day}";
            }
            else if (numberOfDays < 1)
            {
                await DisplayAlert("Invalid day", "Day can not be smaller than 1", "OK");
                Preferences.Default.Set("PayDay", 1);
                Preferences.Default.Set("ClearedPaid", false);
                txtDayOfTheMonth.Text = "1";
            }
            else
            {
                var today = DateTime.Today;
                var firstDayOfThisMonth = new DateTime(today.Year, today.Month, 1);
                var lastDayOfCurrentMonth = firstDayOfThisMonth.AddMonths(1).AddDays(-1);
                var day = firstDayOfThisMonth.AddDays(numberOfDays - 1);
                if ((day < today) && day.Month == DateTime.Today.Month)
                {
                    day = day.AddMonths(1);
                }
                Preferences.Default.Set("PayDay", day);
                Preferences.Default.Set("ClearedPaid", false);
                await DisplayAlert("Updated!", "'Paid' day has been updated.", "OK");
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