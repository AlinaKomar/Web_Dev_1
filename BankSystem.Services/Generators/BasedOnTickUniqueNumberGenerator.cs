using System.Globalization;
using BankSystem.Services.Helpers;

namespace BankSystem.Services.Generators;

/// <summary>
/// Generates unique numbers based on the ticks elapsed since a starting point.
/// </summary>
public class BasedOnTickUniqueNumberGenerator : IUniqueNumberGenerator
{
    private readonly DateTime startingPoint;
    private long lastTicks = long.MinValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="BasedOnTickUniqueNumberGenerator"/> class.
    /// </summary>
    /// <param name="startingPoint">The point in time from which the ticks are counted.</param>
    public BasedOnTickUniqueNumberGenerator(DateTime startingPoint)
    {
        this.startingPoint = startingPoint;
    }

    /// <inheritdoc/>
    public string Generate()
    {
        var now = this.startingPoint.Kind == DateTimeKind.Utc ? DateTime.UtcNow : DateTime.Now;
        long ticks = (now - this.startingPoint).Ticks;

        long previous;
        long next;
        do
        {
            previous = Interlocked.Read(ref this.lastTicks);
            next = ticks > previous ? ticks : previous + 1;
        }
        while (Interlocked.CompareExchange(ref this.lastTicks, next, previous) != previous);

        return next.ToString(CultureInfo.InvariantCulture).GenerateHash();
    }
}
