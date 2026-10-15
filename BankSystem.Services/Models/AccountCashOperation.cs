using System.Globalization;

namespace BankSystem.Services.Models;

public class AccountCashOperation
{
    public AccountCashOperation(decimal amount, DateTime date, string note)
    {
        this.Amount = amount;
        this.Date = date;
        this.Note = note;
    }

    public decimal Amount { get; }

    public DateTime Date { get; }

    public string Note { get; }

    public override string ToString()
    {
        string action = this.Amount < 0 ? "Debited from" : "Credited to";
        return string.Create(
            CultureInfo.InvariantCulture,
            $"{this.Date:MM/dd/yyyy HH:mm:ss} {this.Note} : {action} account {this.Amount}.");
    }
}
