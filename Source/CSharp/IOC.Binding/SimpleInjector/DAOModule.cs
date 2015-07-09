using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleInjector;
using SimpleInjector.Extensions;
using IOC.DAO.Implementation;
using IOC.Abstraction.DAO;
using IOC.FW.Core.Abstraction.Binding;
using IOC.FW.Core.Abstraction.DAO;
using IOC.FW.Core.Database;
using IOC.FW.Core.Base;
using IOC.FW.Core.Database.Repository;
using IOC.Model;

namespace IOC.Binding.SimpleInjector
{
    public class DAOModule
        : IModule
    {
        public void SetBinding(Container container)
        {
            container.RegisterOpenGeneric(
                typeof(IRepository<>),
                typeof(EntityFrameworkRepository<>),
                Lifestyle.Transient
            );

            container.Register<IPersonDAO, PersonDAO>();
            container.Register<INewsDAO, NewsDAO>();
            container.Register<IOcupationDAO, OcupationDAO>();
        }
    }
}
