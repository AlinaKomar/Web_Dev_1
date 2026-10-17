using BankSystem.EF.Entities;
using BankSystem.Services.Models;

namespace BankSystem.Services.Services;

/// <summary>
/// Provides reports about account owners.
/// </summary>
public class OwnerService : BaseService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OwnerService"/> class.
    /// </summary>
    /// <param name="bankContext">The database context used by the service.</param>
    public OwnerService(BankContext bankContext)
        : base(bankContext)
    {
    }

    /// <summary>
    /// Gets the total balance of every account owner in every currency, largest total first.
    /// </summary>
    /// <returns>One row per owner and currency.</returns>
    public IReadOnlyList<AccountOwnerTotalBalanceModel> GetAccountOwnersTotalBalance()
    {
        var accounts = this.Context.BankAccounts
            .Select(account => new
            {
                account.AccountOwnerId,
                account.AccountOwner.FirstName,
                account.AccountOwner.LastName,
                CurrencyCode = account.CurrencyCode.CurrenciesCode,
                account.Balance,
            })
            .ToList();

        return accounts
            .GroupBy(account => new { account.AccountOwnerId, account.FirstName, account.LastName, account.CurrencyCode })
            .Select(group => new AccountOwnerTotalBalanceModel
            {
                AccountOwnerId = group.Key.AccountOwnerId,
                FirstName = group.Key.FirstName,
                LastName = group.Key.LastName,
                CurrencyCode = group.Key.CurrencyCode,
                Total = group.Sum(account => account.Balance),
            })
            .OrderByDescending(model => model.Total)
            .ToList();
    }
}
