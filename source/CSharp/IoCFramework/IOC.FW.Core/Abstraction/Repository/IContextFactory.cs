using System.Data.Common;

namespace IOC.FW.Core.Abstraction.Repository
{
    public interface IContextFactory<TModel>
        where TModel : class, new()
    {
        IContext<TModel> GetContext();

        IContext<TModel> GetContext(
            string nameOrConnectionString
        );

        IContext<TModel> GetContext(
            DbConnection connection,
            bool destroyAfterUse
        );
    }
}