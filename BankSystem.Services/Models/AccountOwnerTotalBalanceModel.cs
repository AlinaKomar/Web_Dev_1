namespace BankSystem.Services.Models;

/// <summary>
/// Represents the total balance of an account owner in one currency.
/// </summary>
public class AccountOwnerTotalBalanceModel
{
    public int AccountOwnerId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string CurrencyCode { get; set; }

    public decimal Total { get; set; }
}
