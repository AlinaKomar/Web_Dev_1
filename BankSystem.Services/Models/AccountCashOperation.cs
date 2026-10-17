using System.Globalization;

namespace BankSystem.Services.Models;

/// <summary>
/// Represents a cash operation performed on a bank account.
/// </summary>
public class AccountCashOperation
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AccountCashOperation"/> class.
    /// </summary>
    /// <param name="amount">The amount of the operation; negative for a debit.</param>
    /// <param name="date">The date and time of the operation.</param>
    /// <param name="note">A note associated with the operation.</param>
    public AccountCashOperation(decimal amount, DateTime date, string note)
    {
        this.Amount = amount;
        this.Date = date;
        this.Note = note;
    }

    /// <summary>
    /// Gets the amount of the operation; negative for a debit.
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// Gets the date and time of the operation.
    /// </summary>
    public DateTime Date { get; }

    /// <summary>
    /// Gets the note associated with the operation.
    /// </summary>
    public string Note { get; }

    /// <inheritdoc/>
    public override string ToString()
    {
        string action = this.Amount < 0 ? "Debited from" : "Credited to";
        return string.Create(
            CultureInfo.InvariantCulture,
            $"{this.Date:MM/dd/yyyy HH:mm:ss} {this.Note} : {action} account {this.Amount}.");
    }
}
