using IOC.FW.Core.Abstraction.Container;

namespace IOC.FW.Core.Abstraction.Repository
{
    public interface IDependencyResolver
    {
        void Resolve(IAdapter adapter);
    }
}