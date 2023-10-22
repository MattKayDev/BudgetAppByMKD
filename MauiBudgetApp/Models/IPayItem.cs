namespace MauiBudgetApp.Models
{
    internal interface IPayItem
    {
        int ID {get; set;}
        string name {get; set;}
        double Amount {get; set;}
        string AmountText => amountText;
        bool IsIncome { get; set; } = false;
        bool IsExpense { get; set; }
        bool IsPaid { get; set; } = false;
        bool IsVisible { get; set; } = false;
    }
}