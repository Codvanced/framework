using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleInjector;
using IOC.FW.Core.Factory;
using IOC.FW.Core.Abstraction.Business;
using IOC.Model;

namespace IOC.Test
{
    public class IBaseBusinessTest
    {
        public static IEnumerable<Container> Configure()
        {
            yield return InstanceFactory.RegisterModules();
        }

        public void BaseTest(Container simpleInjector)
        {
            var business = InstanceFactory.GetImplementation<IBaseBusiness<Person>>();
            var instanceModel = business.Model();
            var instanceListModel = business.List();
        }
    }
}
