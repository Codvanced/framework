using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleInjector;
using SimpleInjector.Extensions;
using IOC.DAO.Implementation;
using IOC.Abstraction.DAO;
using IOC.FW.Core.Abstraction.Repository;
using IOC.FW.Core.Abstraction.DIContainer.Binding;
using IOC.FW.Core.Abstraction.DIContainer;
using IOC.FW.Core.Implementation.Repository.EntityFramework;
using IOC.FW.Core;
using IOC.Model;

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
                Enumerators.LifeCycleType.Transient
            );

            adapter.Register<IPersonDAO, PersonDAO>(Enumerators.LifeCycleType.Transient);
            adapter.Register<INewsDAO, NewsDAO>(Enumerators.LifeCycleType.Transient);
            adapter.Register<IOcupationDAO, OcupationDAO>(Enumerators.LifeCycleType.Transient);
        }
    }
}
