using IOC.FW.Abstraction.Container;

namespace IOC.FW.Abstraction.Repository
{
    public interface IDependencyResolver
    {
        void Resolve(IAdapter adapter);
    }
}