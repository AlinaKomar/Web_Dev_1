namespace BankSystem.Services.Models;

/// <summary>
/// Represents full information about a bank account together with its owner.
/// </summary>
public class BankAccountFullInfoModel
{
    public int BankAccountId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string AccountNumber { get; set; }

    public decimal Balance { get; set; }

    public string CurrencyCode { get; set; }

    public int BonusPoints { get; set; }
}
