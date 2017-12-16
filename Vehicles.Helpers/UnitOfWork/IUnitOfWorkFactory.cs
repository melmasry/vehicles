

namespace Vehicles.Helpers.UnitOfWork
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork GetInstance();
    }
}
