using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using IOC.FW.Core.Abstraction.Repository;
using IOC.FW.Repository.EntityFramework;

namespace IOC.FW.Repository
{
    public class ContextFactory<TModel>
        : IContextFactory<TModel>
        where TModel : class, new()
    {
        public IContext<TModel> GetContext()
        {
            return new EntityFrameworkContext<TModel>();
        }

        public IContext<TModel> GetContext(
            string nameOrConnectionString
        )
        {
            return new EntityFrameworkContext<TModel>(nameOrConnectionString);
        }

        public IContext<TModel> GetContext(
            DbConnection connection,
            bool destroyAfterUse
        )
        {
            return new EntityFrameworkContext<TModel>(connection, destroyAfterUse);
        }
    }
}