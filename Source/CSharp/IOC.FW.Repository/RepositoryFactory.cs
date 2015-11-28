using System.Linq;
using IOC.FW.Core.Abstraction.Repository;
using IOC.FW.Shared.Enumerators;
using IOC.FW.Core.Abstraction.Container;

namespace IOC.FW.Repository
{
    public class RepositoryFactory<TModel>
        : IRepositoryFactory<TModel>
        where TModel : class, new()
    {
        private readonly IAdapter _adapter;

        public RepositoryFactory(IAdapter adapter)
        {
            _adapter = adapter;
        }

        public IRepository<TModel> Resolve(RepositoryEnumerator.RepositoryType type)
        {
            var repository = default(IRepository<TModel>);
            var instances = _adapter.ResolveMany<IRepository<TModel>>();

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