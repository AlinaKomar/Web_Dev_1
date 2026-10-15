using System.Globalization;
using BankSystem.Services.Helpers;

namespace BankSystem.Services.Generators;

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

    public static SimpleGenerator Instance { get; }

    public string Generate()
    {
        long value = Interlocked.Increment(ref this.counter);
        return value.ToString(CultureInfo.InvariantCulture).GenerateHash();
    }
}
