namespace Thermostat.Application.Common.DbContext
{
    // In Application Layer (or Core)
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransactionAsync(CancellationToken ct = default);
        Task CommitAsync(CancellationToken ct = default);
        Task RollbackAsync(CancellationToken ct = default);
    }

    public interface IMeasurementDBUnitOfWork : IUnitOfWork
    {
        // Additional methods specific to Measurement DB can be added here
    }
}
