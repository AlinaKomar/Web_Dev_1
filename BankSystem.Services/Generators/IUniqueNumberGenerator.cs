namespace BankSystem.Services.Generators;

/// <summary>
/// Defines a contract for generating unique strings used as bank account numbers.
/// </summary>
public interface IUniqueNumberGenerator
{
    /// <summary>
    /// Generates a string that represents a unique number.
    /// </summary>
    /// <returns>A unique string.</returns>
    string Generate();
}
