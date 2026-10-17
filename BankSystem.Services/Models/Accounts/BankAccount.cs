using System.Globalization;
using BankSystem.Services.Generators;
using BankSystem.Services.Helpers;

namespace BankSystem.Services.Models.Accounts;

/// <summary>
/// Represents a base bank account.
/// </summary>
public abstract class BankAccount
{
    private readonly List<AccountCashOperation> operations = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="BankAccount"/> class.
    /// </summary>
    /// <param name="owner">The owner of the account.</param>
    /// <param name="currencyCode">The ISO currency code of the account.</param>
    /// <param name="uniqueNumberGenerator">The generator of the account number.</param>
    protected BankAccount(AccountOwner owner, string currencyCode, IUniqueNumberGenerator uniqueNumberGenerator)
        : this(owner, currencyCode, ToDelegate(uniqueNumberGenerator))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BankAccount"/> class.
    /// </summary>
    /// <param name="owner">The owner of the account.</param>
    /// <param name="currencyCode">The ISO currency code of the account.</param>
    /// <param name="numberGenerator">The function that generates the account number.</param>
    protected BankAccount(AccountOwner owner, string currencyCode, Func<string> numberGenerator)
    {
        this.Initialize(owner, currencyCode, numberGenerator);
        owner.Add(this);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BankAccount"/> class with an opening deposit.
    /// </summary>
    /// <param name="owner">The owner of the account.</param>
    /// <param name="currencyCode">The ISO currency code of the account.</param>
    /// <param name="uniqueNumberGenerator">The generator of the account number.</param>
    /// <param name="initialBalance">The amount of the opening deposit.</param>
    protected BankAccount(AccountOwner owner, string currencyCode, IUniqueNumberGenerator uniqueNumberGenerator, decimal initialBalance)
        : this(owner, currencyCode, ToDelegate(uniqueNumberGenerator), initialBalance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BankAccount"/> class with an opening deposit.
    /// </summary>
    /// <param name="owner">The owner of the account.</param>
    /// <param name="currencyCode">The ISO currency code of the account.</param>
    /// <param name="numberGenerator">The function that generates the account number.</param>
    /// <param name="initialBalance">The amount of the opening deposit.</param>
    protected BankAccount(AccountOwner owner, string currencyCode, Func<string> numberGenerator, decimal initialBalance)
    {
        this.Initialize(owner, currencyCode, numberGenerator);

        this.Deposit(initialBalance, DateTime.Now, "Initial balance");
        owner.Add(this);
    }

    /// <summary>
    /// Gets the account number.
    /// </summary>
    public string Number { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the balance of the account.
    /// </summary>
    public decimal Balance { get; private set; }

    /// <summary>
    /// Gets the ISO currency code of the account.
    /// </summary>
    public string CurrencyCode { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the owner of the account.
    /// </summary>
    public AccountOwner AccountOwner { get; private set; } = null!;

    /// <summary>
    /// Gets the bonus points of the account.
    /// </summary>
    public int BonusPoints { get; private set; }

    /// <summary>
    /// Gets the overdraft limit of the account.
    /// </summary>
    protected abstract decimal Overdraft { get; }

    /// <summary>
    /// Deposits money to the account.
    /// </summary>
    /// <param name="amount">The positive amount to deposit.</param>
    /// <param name="date">The date and time of the operation.</param>
    /// <param name="note">A note associated with the operation.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the amount is not positive.</exception>
    public void Deposit(decimal amount, DateTime date, string note)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "The deposit amount must be positive.");
        }

        this.Balance += amount;
        this.operations.Add(new AccountCashOperation(amount, date, note));
        this.BonusPoints += this.CalculateDepositRewardPoints(amount);
    }

    /// <summary>
    /// Withdraws money from the account.
    /// </summary>
    /// <param name="amount">The positive amount to withdraw.</param>
    /// <param name="date">The date and time of the operation.</param>
    /// <param name="note">A note associated with the operation.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the amount is not positive.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the amount exceeds the balance plus the overdraft.</exception>
    public void Withdraw(decimal amount, DateTime date, string note)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "The withdrawal amount must be positive.");
        }

        if (amount > this.Balance + this.Overdraft)
        {
            throw new InvalidOperationException("The withdrawal amount exceeds the balance and the overdraft limit.");
        }

        this.Balance -= amount;
        this.operations.Add(new AccountCashOperation(-amount, date, note));
        this.BonusPoints += this.CalculateWithdrawRewardPoints(amount);
    }

    /// <summary>
    /// Gets all operations performed on the account.
    /// </summary>
    /// <returns>A list of the cash operations.</returns>
    public IList<AccountCashOperation> GetAllOperations()
    {
        return [.. this.operations];
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return string.Create(
            CultureInfo.InvariantCulture,
            $"{this.Overdraft} {this.AccountOwner} No:{this.Number}. Balance: {this.Balance}{this.CurrencyCode}.\n");
    }

    /// <summary>
    /// Calculates the bonus points for a deposit. The <see cref="Balance"/> is already updated.
    /// </summary>
    /// <param name="amount">The deposited amount.</param>
    /// <returns>The bonus points to add.</returns>
    protected abstract int CalculateDepositRewardPoints(decimal amount);

    /// <summary>
    /// Calculates the bonus points for a withdrawal. The <see cref="Balance"/> is already updated.
    /// </summary>
    /// <param name="amount">The withdrawn amount.</param>
    /// <returns>The bonus points to add.</returns>
    protected abstract int CalculateWithdrawRewardPoints(decimal amount);

    private static Func<string> ToDelegate(IUniqueNumberGenerator uniqueNumberGenerator)
    {
        ArgumentNullException.ThrowIfNull(uniqueNumberGenerator);
        return uniqueNumberGenerator.Generate;
    }

    private void Initialize(AccountOwner owner, string currencyCode, Func<string> numberGenerator)
    {
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(numberGenerator);

        if (!currencyCode.IsCurrencyValid())
        {
            throw new ArgumentException("The currency code must be an existing ISO currency code.", nameof(currencyCode));
        }

        this.AccountOwner = owner;
        this.CurrencyCode = currencyCode;
        this.Number = numberGenerator();
    }
}
