using System.Linq;
using IOC.FW.Core.Abstraction.Repository;
using IOC.FW.Core.Implementation.DIContainer;

namespace IOC.FW.Core.Implementation.Repository
{
    public class RepositoryFactory<TModel>
        : IRepositoryFactory<TModel>
        where TModel : class, new()
    {
        public IRepository<TModel> Resolve(Enumerators.RepositoryType type)
        {
            var repository = default(IRepository<TModel>);
            var instances = DependencyResolver.Adapter.ResolveMany<IRepository<TModel>>();

            if (instances != null
                && instances.Count() > 0
            )
            {
                repository = instances.FirstOrDefault(
                    i => i.Type == type
                );
            }

            return repository;
        }
    }
}