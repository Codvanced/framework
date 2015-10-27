﻿using System.Linq;
using NUnit.Framework;
using SimpleInjector;
using IOC.Abstraction.Business;
using IOC.Model;
using IOC.FW.ContainerManager.SimpleInjector;

namespace IOC.Test
{
    [TestFixture]
    public class PriorityTest
    {
        public static void Configure()
        {
        }

        [Test, TestCaseSource("Configure")]
        public void ChangePriorityTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var adapter = new SimpleInjectorAdapter();
            var business = adapter.Resolve<INewsBusiness>();
            Assert.NotNull(business);

            var items = business.SelectAll();

            Assert.IsNotEmpty(items);

            business.UpdatePriority<News>(items.ToArray());

            var pr = items[0].Priority;

            for (int i = 1; i < items.Count; i++)
            {
                Assert.Less(items[i].Priority, pr);
                pr = items[i].Priority;
            }
        }
    }
}
