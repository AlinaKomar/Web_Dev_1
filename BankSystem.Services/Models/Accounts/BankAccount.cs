using System.Globalization;
using BankSystem.Services.Generators;
using BankSystem.Services.Helpers;

namespace BankSystem.Services.Models.Accounts;

public abstract class BankAccount
{
    private readonly List<AccountCashOperation> operations = [];

    protected BankAccount(AccountOwner owner, string currencyCode, IUniqueNumberGenerator uniqueNumberGenerator)
        : this(owner, currencyCode, ToDelegate(uniqueNumberGenerator))
    {
    }

    protected BankAccount(AccountOwner owner, string currencyCode, Func<string> numberGenerator)
    {
        this.Initialize(owner, currencyCode, numberGenerator);
        owner.Add(this);
    }

    protected BankAccount(AccountOwner owner, string currencyCode, IUniqueNumberGenerator uniqueNumberGenerator, decimal initialBalance)
        : this(owner, currencyCode, ToDelegate(uniqueNumberGenerator), initialBalance)
    {
    }

    protected BankAccount(AccountOwner owner, string currencyCode, Func<string> numberGenerator, decimal initialBalance)
    {
        this.Initialize(owner, currencyCode, numberGenerator);

        this.Deposit(initialBalance, DateTime.Now, "Initial balance");
        owner.Add(this);
    }

    public string Number { get; private set; } = string.Empty;

    public decimal Balance { get; private set; }

    public string CurrencyCode { get; private set; } = string.Empty;

    public AccountOwner AccountOwner { get; private set; } = null!;

    public int BonusPoints { get; private set; }

    protected abstract decimal Overdraft { get; }

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

    public IList<AccountCashOperation> GetAllOperations()
    {
        return [.. this.operations];
    }

    public override string ToString()
    {
        return string.Create(
            CultureInfo.InvariantCulture,
            $"{this.Overdraft} {this.AccountOwner} No:{this.Number}. Balance: {this.Balance}{this.CurrencyCode}.\n");
    }

    protected abstract int CalculateDepositRewardPoints(decimal amount);

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
