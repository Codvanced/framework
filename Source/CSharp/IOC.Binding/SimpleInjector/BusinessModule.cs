using IOC.Business.Implementation;
using IOC.Abstraction.Business;
using IOC.FW.Core.Abstraction.Container.Binding;
using IOC.FW.Core.Abstraction.Container;
using IOC.FW.Shared.Enumerators;

namespace IOC.Binding.SimpleInjector
{
    public class BusinessModule
        : IBinding
    {
        public void SetBinding(IAdapter adapter)
        {
            adapter.Register(
                typeof(IPersonBusiness),
                typeof(PersonBusiness),
                ContainerEnumerator.LifeCycle.Transient
            );

            adapter.Register(
                typeof(INewsBusiness),
                typeof(NewsBusiness),
                ContainerEnumerator.LifeCycle.Transient
            );

            adapter.Register(
                typeof(IOcupationBusiness),
                typeof(OcupationBusiness),
                ContainerEnumerator.LifeCycle.Transient
            );
        }
    }
}