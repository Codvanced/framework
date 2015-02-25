using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SimpleInjector;
using IOC.FW.Core.Factory;
using IOC.Abstraction.Business;
using IOC.Model;

namespace IOC.Test
{
    [TestFixture]
    public class PriorityTest
    {
        public static IEnumerable<Container> Configure()
        {
            yield return InstanceFactory.RegisterModules();
        }

        [Test, TestCaseSource("Configure")]
        public void ChangePriorityTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<AbstractNoticiaBusiness>();
            Assert.NotNull(business);

            var items = business.SelectAll();

            Assert.IsNotEmpty(items);

            business.UpdatePriority<Noticia>(items.ToArray());

            var pr = items[0].Priority;

            for (int i = 1; i < items.Count; i++)
            {
                Assert.Less(items[i].Priority, pr);
                pr = items[i].Priority;
            }
        }
    }
}
