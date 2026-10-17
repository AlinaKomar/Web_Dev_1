using BankSystem.Services.Generators;

namespace BankSystem.Services.Models.Accounts;

/// <summary>
/// Represents a silver bank account.
/// </summary>
public class SilverAccount : BankAccount
{
    private const int SilverDepositCostPerPoint = 5;
    private const int SilverWithdrawCostPerPoint = 2;
    private const int SilverBalanceCostPerPoint = 100;

    /// <summary>
    /// Initializes a new instance of the <see cref="SilverAccount"/> class.
    /// </summary>
    /// <param name="owner">The owner of the account.</param>
    /// <param name="currencyCode">The ISO currency code of the account.</param>
    /// <param name="uniqueNumberGenerator">The generator of the account number.</param>
    public SilverAccount(AccountOwner owner, string currencyCode, IUniqueNumberGenerator uniqueNumberGenerator)
        : base(owner, currencyCode, uniqueNumberGenerator)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SilverAccount"/> class.
    /// </summary>
    /// <param name="owner">The owner of the account.</param>
    /// <param name="currencyCode">The ISO currency code of the account.</param>
    /// <param name="numberGenerator">The function that generates the account number.</param>
    public SilverAccount(AccountOwner owner, string currencyCode, Func<string> numberGenerator)
        : base(owner, currencyCode, numberGenerator)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SilverAccount"/> class with an opening deposit.
    /// </summary>
    /// <param name="owner">The owner of the account.</param>
    /// <param name="currencyCode">The ISO currency code of the account.</param>
    /// <param name="uniqueNumberGenerator">The generator of the account number.</param>
    /// <param name="initialBalance">The amount of the opening deposit.</param>
    public SilverAccount(AccountOwner owner, string currencyCode, IUniqueNumberGenerator uniqueNumberGenerator, decimal initialBalance)
        : base(owner, currencyCode, uniqueNumberGenerator, initialBalance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SilverAccount"/> class with an opening deposit.
    /// </summary>
    /// <param name="owner">The owner of the account.</param>
    /// <param name="currencyCode">The ISO currency code of the account.</param>
    /// <param name="numberGenerator">The function that generates the account number.</param>
    /// <param name="initialBalance">The amount of the opening deposit.</param>
    public SilverAccount(AccountOwner owner, string currencyCode, Func<string> numberGenerator, decimal initialBalance)
        : base(owner, currencyCode, numberGenerator, initialBalance)
    {
    }

    /// <inheritdoc/>
    protected override decimal Overdraft => 2 * this.BonusPoints;

    /// <inheritdoc/>
    protected override int CalculateDepositRewardPoints(decimal amount)
    {
        return (int)Math.Max(Math.Floor((this.Balance / SilverBalanceCostPerPoint) + (amount / SilverDepositCostPerPoint)), 0m);
    }

    /// <inheritdoc/>
    protected override int CalculateWithdrawRewardPoints(decimal amount)
    {
        return (int)Math.Max(Math.Floor((this.Balance / SilverBalanceCostPerPoint) + (amount / SilverWithdrawCostPerPoint)), 0m);
    }
}
