using BankSystem.EF.Entities;
using BankSystem.Services.Models;

namespace BankSystem.Services.Services;

/// <summary>
/// Provides reports about bank accounts.
/// </summary>
public class AccountService : BaseService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AccountService"/> class.
    /// </summary>
    /// <param name="bankContext">The database context used by the service.</param>
    public AccountService(BankContext bankContext)
        : base(bankContext)
    {
    }

    /// <summary>
    /// Gets full information about every bank account together with its owner and currency.
    /// </summary>
    /// <returns>One row per bank account.</returns>
    public IReadOnlyList<BankAccountFullInfoModel> GetBankAccountsFullInfo()
    {
        return this.Context.BankAccounts
            .Select(account => new BankAccountFullInfoModel
            {
                BankAccountId = account.Id,
                FirstName = account.AccountOwner.FirstName,
                LastName = account.AccountOwner.LastName,
                AccountNumber = account.Number,
                Balance = account.Balance,
                CurrencyCode = account.CurrencyCode.CurrenciesCode,
                BonusPoints = account.BonusPoints,
            })
            .ToList();
    }
}
