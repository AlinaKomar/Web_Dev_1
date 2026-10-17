using BankSystem.Services.Helpers;
using BankSystem.Services.Models.Accounts;

namespace BankSystem.Services.Models;

/// <summary>
/// Represents an owner of bank accounts.
/// </summary>
public class AccountOwner
{
    private readonly List<BankAccount> accounts = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountOwner"/> class.
    /// </summary>
    /// <param name="firstName">The first name of the owner.</param>
    /// <param name="lastName">The last name of the owner.</param>
    /// <param name="email">The email address of the owner.</param>
    /// <exception cref="ArgumentException">Thrown when a name is null, empty or white space, or the email is not valid.</exception>
    public AccountOwner(string firstName, string lastName, string email)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Email = email;
    }

    /// <summary>
    /// Gets or sets the first name of the owner.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the value is null, empty or white space.</exception>
    public string FirstName
    {
        get;
        set => field = RequireName(value, nameof(value));
    } = string.Empty;

    /// <summary>
    /// Gets or sets the last name of the owner.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the value is null, empty or white space.</exception>
    public string LastName
    {
        get;
        set => field = RequireName(value, nameof(value));
    } = string.Empty;

    /// <summary>
    /// Gets or sets the email address of the owner.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the value is not a valid email address.</exception>
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

    /// <summary>
    /// Adds an account to the accounts of the owner.
    /// </summary>
    /// <param name="account">The account to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when the account is null.</exception>
    public void Add(BankAccount account)
    {
        ArgumentNullException.ThrowIfNull(account);
        this.accounts.Add(account);
    }

    /// <summary>
    /// Gets the accounts of the owner.
    /// </summary>
    /// <returns>A list of the accounts that belong to the owner.</returns>
    public IList<BankAccount> Accounts()
    {
        return [.. this.accounts];
    }

    /// <inheritdoc/>
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
