namespace MauiBudgetApp.Models
{
    public interface IPayItem
    {
        int ID {get; set;}
        string Name {get; set;}
        decimal Amount {get; set;}

        string AmountText { get; }
    }
}