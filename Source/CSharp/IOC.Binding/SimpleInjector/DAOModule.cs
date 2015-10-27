using IOC.DAO.Implementation;
using IOC.Abstraction.DAO;
using IOC.FW.Core.Abstraction.Repository;
using IOC.FW.Core.Abstraction.Container.Binding;
using IOC.FW.Core.Abstraction.Container;
using IOC.FW.Shared.Enumerators;
using IOC.FW.Repository.EntityFramework;

namespace IOC.Binding.SimpleInjector
{
    public class DAOModule
        : IBinding
    {
        public void SetBinding(IAdapter adapter)
        {
            adapter.Register(
                typeof(IRepository<>),
                typeof(EntityFrameworkRepository<>),
                ContainerEnumerator.LifeCycle.Transient
            );

            adapter.Register<IPersonDAO, PersonDAO>(ContainerEnumerator.LifeCycle.Transient);
            adapter.Register<INewsDAO, NewsDAO>(ContainerEnumerator.LifeCycle.Transient);
            adapter.Register<IOcupationDAO, OcupationDAO>(ContainerEnumerator.LifeCycle.Transient);
        }
    }
}
