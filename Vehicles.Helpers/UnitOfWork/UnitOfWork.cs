using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Data.SqlClient;
using Vehicles.Entities.HelperEntities;
using Vehicles.Repositories.DomainRepositories;

namespace Vehicles.Helpers.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool _disposed;

        public ICustomersRepository CustomersRepository { get; set; }
        public IVehiclesRepository VehiclesRepository { get; set; }
        
        public UnitOfWork(IOptions<DatabaseOptions> databaseOptions, ICustomersRepository customersRepository, IVehiclesRepository vehiclesRepository)
        {
            _connection = customersRepository.Connection =  vehiclesRepository.Connection = new SqlConnection(databaseOptions.Value.ConnectionString);
            _connection.Open();
            _transaction = customersRepository.Transaction = vehiclesRepository.Transaction = _connection.BeginTransaction(IsolationLevel.ReadCommitted);

            CustomersRepository = customersRepository;
            VehiclesRepository = vehiclesRepository;
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = _connection.BeginTransaction();
        }
        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
            }
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }
        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            dispose(false);
        }
    }
}
