using System.Globalization;
using BankSystem.Services.Helpers;

namespace BankSystem.Services.Generators;

/// <summary>
/// Generates sequential numbers. Implemented as a singleton.
/// </summary>
public sealed class SimpleGenerator : IUniqueNumberGenerator
{
    private long counter = 1234567890;

    static SimpleGenerator()
    {
        Instance = new SimpleGenerator();
    }

    private SimpleGenerator()
    {
    }

    /// <summary>
    /// Gets the single instance of the <see cref="SimpleGenerator"/> class.
    /// </summary>
    public static SimpleGenerator Instance { get; }

    /// <inheritdoc/>
    public string Generate()
    {
        long value = Interlocked.Increment(ref this.counter);
        return value.ToString(CultureInfo.InvariantCulture).GenerateHash();
    }
}
