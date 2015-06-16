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
using IOC.Interface.Business;

namespace IOC.Binding.SimpleInjector
{
    public class BusinessModule
        : IModule
    {
        public void SetBinding(Container container)
        {
            container.Register<PersonBusinessAbstract, PersonBusiness>(Lifestyle.Singleton);
            container.Register<NewsBusinessAbstract, NewsBusiness>(Lifestyle.Singleton);
            container.Register<OcupationBusinessAbstract, OcupationBusiness>(Lifestyle.Singleton);
        }
    }
}