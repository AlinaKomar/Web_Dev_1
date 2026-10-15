using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BankSystem.Services.Helpers;

public static class ValidationService
{
    private static readonly Lazy<HashSet<string>> IsoCurrencySymbols = new(LoadIsoCurrencySymbols);

    public static bool IsCurrencyValid(this string currency)
    {
        return !string.IsNullOrWhiteSpace(currency) && IsoCurrencySymbols.Value.Contains(currency);
    }

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
