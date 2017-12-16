using System;
using Vehicles.Repositories.DomainRepositories;

namespace Vehicles.Helpers.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomersRepository CustomersRepository { get; }
        IVehiclesRepository VehiclesRepository { get; }
        void Rollback();
        void Commit();
    }
}
