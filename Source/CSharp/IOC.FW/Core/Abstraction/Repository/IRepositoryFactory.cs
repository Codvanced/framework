using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOC.FW.Core.Abstraction.Repository
{
    public interface IRepositoryFactory<TModel>
        where TModel : class, new()
    {
        IRepository<TModel> Resolve(
            Enumerators.RepositoryType type
        );
    }
}