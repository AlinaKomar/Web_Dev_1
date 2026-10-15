using BankSystem.Services.Helpers;
using BankSystem.Services.Models.Accounts;

namespace BankSystem.Services.Models;

public class AccountOwner
{
    private readonly List<BankAccount> accounts = [];

    public AccountOwner(string firstName, string lastName, string email)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Email = email;
    }

    public string FirstName
    {
        get;
        set => field = RequireName(value, nameof(value));
    } = string.Empty;

    public string LastName
    {
        get;
        set => field = RequireName(value, nameof(value));
    } = string.Empty;

    public string Email
    {
        get;
        set
        {
            if (!value.IsEmailValid())
            {
                throw new ArgumentException("The email is not a valid email address.", nameof(value));
            }

            field = value;
        }
    } = string.Empty;

    public void Add(BankAccount account)
    {
        ArgumentNullException.ThrowIfNull(account);
        this.accounts.Add(account);
    }

    public IList<BankAccount> Accounts()
    {
        return [.. this.accounts];
    }

    public override string ToString()
    {
        return $"{this.FirstName} {this.LastName}, {this.Email}.";
    }

    private static string RequireName(string? value, string parameterName)
    {
        return string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("The name cannot be null, empty or white space.", parameterName)
            : value;
    }
}
