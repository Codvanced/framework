using IOC.FW.Data;
using IOC.FW.Shared.Enumerators;

namespace IOC.FW.Abstraction.Repository
{
    public interface IRepositoryFactory<TModel>
        where TModel : class, new()
    {
        IRepository<TModel> Resolve(
            Enumerators.RepositoryType type
        );
    }
}