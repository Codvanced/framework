using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleInjector;
using SimpleInjector.Extensions;
using IOC.Business.Implementation;
using IOC.FW.Core.Abstraction.Binding;
using IOC.Model;
using IOC.Abstraction.Business;
using IOC.FW.Core.Base;
using IOC.FW.Core.Abstraction.Business;

namespace IOC.Binding.SimpleInjector
{
    public class BusinessModule
        : IModule
    {
        public void SetBinding(Container container)
        {
            container.Register<IPersonBusiness, PersonBusiness>(Lifestyle.Singleton);
            container.Register<INewsBusiness, NewsBusiness>(Lifestyle.Singleton);
            container.Register<IOcupationBusiness, OcupationBusiness>(Lifestyle.Singleton);
        }
    }
}