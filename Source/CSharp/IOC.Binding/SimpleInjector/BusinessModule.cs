using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleInjector;
using SimpleInjector.Extensions;
using IOC.Business.Implementation;
using IOC.Model;
using IOC.Abstraction.Business;
using IOC.FW.Core.Abstraction.Business;
using IOC.FW.Core.Abstraction.DIContainer.Binding;
using IOC.FW.Core.Abstraction.DIContainer;
using IOC.FW.Core;

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
                Enumerators.LifeCycleType.Transient
            );

            adapter.Register(
                typeof(INewsBusiness),
                typeof(NewsBusiness),
                Enumerators.LifeCycleType.Transient
            );

            adapter.Register(
                typeof(IOcupationBusiness),
                typeof(OcupationBusiness),
                Enumerators.LifeCycleType.Transient
            );
        }
    }
}