using BankSystem.Services.Generators;

namespace BankSystem.Services.Models.Accounts;

/// <summary>
/// Represents a gold bank account.
/// </summary>
public class GoldAccount : BankAccount
{
    private const int GoldDepositCostPerPoint = 10;
    private const int GoldWithdrawCostPerPoint = 5;
    private const int GoldBalanceCostPerPoint = 5;

    /// <summary>
    /// Initializes a new instance of the <see cref="GoldAccount"/> class.
    /// </summary>
    /// <param name="owner">The owner of the account.</param>
    /// <param name="currencyCode">The ISO currency code of the account.</param>
    /// <param name="uniqueNumberGenerator">The generator of the account number.</param>
    public GoldAccount(AccountOwner owner, string currencyCode, IUniqueNumberGenerator uniqueNumberGenerator)
        : base(owner, currencyCode, uniqueNumberGenerator)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GoldAccount"/> class.
    /// </summary>
    /// <param name="owner">The owner of the account.</param>
    /// <param name="currencyCode">The ISO currency code of the account.</param>
    /// <param name="numberGenerator">The function that generates the account number.</param>
    public GoldAccount(AccountOwner owner, string currencyCode, Func<string> numberGenerator)
        : base(owner, currencyCode, numberGenerator)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GoldAccount"/> class with an opening deposit.
    /// </summary>
    /// <param name="owner">The owner of the account.</param>
    /// <param name="currencyCode">The ISO currency code of the account.</param>
    /// <param name="uniqueNumberGenerator">The generator of the account number.</param>
    /// <param name="initialBalance">The amount of the opening deposit.</param>
    public GoldAccount(AccountOwner owner, string currencyCode, IUniqueNumberGenerator uniqueNumberGenerator, decimal initialBalance)
        : base(owner, currencyCode, uniqueNumberGenerator, initialBalance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GoldAccount"/> class with an opening deposit.
    /// </summary>
    /// <param name="owner">The owner of the account.</param>
    /// <param name="currencyCode">The ISO currency code of the account.</param>
    /// <param name="numberGenerator">The function that generates the account number.</param>
    /// <param name="initialBalance">The amount of the opening deposit.</param>
    public GoldAccount(AccountOwner owner, string currencyCode, Func<string> numberGenerator, decimal initialBalance)
        : base(owner, currencyCode, numberGenerator, initialBalance)
    {
    }

    /// <inheritdoc/>
    protected override decimal Overdraft => 3 * this.BonusPoints;

    /// <inheritdoc/>
    protected override int CalculateDepositRewardPoints(decimal amount)
    {
        return (int)Math.Max(Math.Ceiling((this.Balance / GoldBalanceCostPerPoint) + (amount / GoldDepositCostPerPoint)), 0m);
    }

    /// <inheritdoc/>
    protected override int CalculateWithdrawRewardPoints(decimal amount)
    {
        return (int)Math.Max(Math.Ceiling((this.Balance / GoldBalanceCostPerPoint) + (amount / GoldWithdrawCostPerPoint)), 0m);
    }
}
