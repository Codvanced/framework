using IOC.FW.Abstraction.Container;

namespace IOC.FW.Abstraction.Container
{
    public interface IDependencyResolver
    {
        void Resolve(IAdapter adapter);
    }
}