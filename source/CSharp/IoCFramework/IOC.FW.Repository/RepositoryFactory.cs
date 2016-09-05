using System.Linq;
using IOC.FW.Abstraction.Repository;
using IOC.FW.Data;
using System.Collections.Generic;

namespace IOC.FW.Repository
{
    public class RepositoryFactory<TModel>
        : IRepositoryFactory<TModel>
        where TModel : class, new()
    {
        private readonly IEnumerable<IRepository<TModel>> _repositories;

        public RepositoryFactory(IEnumerable<IRepository<TModel>> repositories)
        {
            _repositories = repositories;
        }

        public IRepository<TModel> Resolve(Enumerators.RepositoryType type)
        {
            var repository = default(IRepository<TModel>);

            if (_repositories != null
                && _repositories.Count() > 0
            )
            {
                repository = _repositories.FirstOrDefault(
                    i => i.Type == type
                );
            }

            return repository;
        }
    }
}