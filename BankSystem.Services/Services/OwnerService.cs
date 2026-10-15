using BankSystem.EF.Entities;
using BankSystem.Services.Models;

namespace BankSystem.Services.Services;

public class OwnerService : BaseService
{
    public OwnerService(BankContext bankContext)
        : base(bankContext)
    {
    }

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
