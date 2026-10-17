using System.Globalization;
using BankSystem.Services.Helpers;

namespace BankSystem.Services.Generators;

/// <summary>
/// Generates unique numbers based on globally unique identifiers.
/// </summary>
public class GuidNumberGenerator : IUniqueNumberGenerator
{
    /// <inheritdoc/>
    public string Generate()
    {
        return Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture).GenerateHash();
    }
}
