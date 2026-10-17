using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BankSystem.Services.Helpers;

/// <summary>
/// Provides validation helpers for currencies and email addresses.
/// </summary>
public static class ValidationService
{
    private static readonly Lazy<HashSet<string>> IsoCurrencySymbols = new(LoadIsoCurrencySymbols);

    /// <summary>
    /// Checks whether the specified string is an existing ISO currency code.
    /// </summary>
    /// <param name="currency">The currency code to check, for example "USD".</param>
    /// <returns><see langword="true"/> if the code is a known ISO currency code; otherwise <see langword="false"/>.</returns>
    public static bool IsCurrencyValid(this string currency)
    {
        return !string.IsNullOrWhiteSpace(currency) && IsoCurrencySymbols.Value.Contains(currency);
    }

    /// <summary>
    /// Checks whether the specified string is a correctly formatted email address.
    /// </summary>
    /// <param name="email">The email address to check.</param>
    /// <returns><see langword="true"/> if the email has a valid format; otherwise <see langword="false"/>.</returns>
    public static bool IsEmailValid(this string email)
    {
        return !string.IsNullOrWhiteSpace(email) && new EmailAddressAttribute().IsValid(email);
    }

    private static HashSet<string> LoadIsoCurrencySymbols()
    {
        var symbols = new HashSet<string>(StringComparer.Ordinal);

        foreach (var culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
        {
            _ = symbols.Add(new RegionInfo(culture.Name).ISOCurrencySymbol);
        }

        return symbols;
    }
}
