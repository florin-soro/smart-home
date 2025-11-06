using MongoDB.Driver;
using Thermostat.Application.Common.DbContext;
using Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.DbContext.Exceptions;

namespace Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.DbContext
{
    public class MongoDbContextBase: IUnitOfWork
    {
        protected readonly IMongoDatabase _database;
        protected readonly IMongoClient mongoClient;
        protected IClientSessionHandle _session;
        private bool _disposed;
        public MongoDbContextBase(IMongoClient mongoClient, string databaseName)
        {
            _database = mongoClient.GetDatabase(databaseName);
            this.mongoClient = mongoClient;
            _disposed = false;
        }

        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            if (_session != null)
                throw new TransactionAllreadyInProgressException("Transaction already in progress");

            _session = await mongoClient.StartSessionAsync(cancellationToken: ct);
            _session.StartTransaction();
        }

        public async Task CommitAsync(CancellationToken ct = default)
        {
            if (_session == null || !_session.IsInTransaction)
                throw new InvalidOperationException("No active transaction to commit");

            try
            {
                await _session.CommitTransactionAsync(ct);
            }
            finally
            {
                DisposeSession();
            }
        }

        public async Task RollbackAsync(CancellationToken ct = default)
        {
            if (_session == null || !_session.IsInTransaction)
                throw new InvalidOperationException("No active transaction to rollback");

            try
            {
                await _session.AbortTransactionAsync(ct);
            }
            finally
            {
                DisposeSession();
            }
        }
        private void DisposeSession()
        {
            _session?.Dispose();
            _session = null;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    DisposeSession();
                }
                _disposed = true;
            }
        }
    }
}
