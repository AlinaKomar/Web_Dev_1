using BankSystem.EF.Entities;

namespace BankSystem.Services.Services;

/// <summary>
/// Base class for the services that work with the bank database.
/// </summary>
public class BaseService : IDisposable
{
    private bool disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseService"/> class.
    /// </summary>
    /// <param name="bankContext">The database context used by the service.</param>
    /// <exception cref="ArgumentNullException">Thrown when the context is null.</exception>
    public BaseService(BankContext bankContext)
    {
        ArgumentNullException.ThrowIfNull(bankContext);
        this.Context = bankContext;
    }

    /// <summary>
    /// Gets the database context used by the service.
    /// </summary>
    protected BankContext Context { get; }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the resources used by the service.
    /// </summary>
    /// <param name="disposing"><see langword="true"/> to release managed resources; otherwise <see langword="false"/>.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (this.disposed)
        {
            return;
        }

        if (disposing)
        {
            this.Context.Dispose();
        }

        this.disposed = true;
    }
}
