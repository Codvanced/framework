using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleInjector;
using IOC.FW.Core.Factory;
using IOC.FW.Core.Abstraction.Business;
using IOC.Model;
using System.Linq.Expressions;
using IOC.FW.Core.Database;
using System.Data.Entity;
using System.Collections;
using NUnit.Framework;

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

        public void OrderByTest()
        {
            var business = InstanceFactory.GetImplementation<IBaseBusiness<Person>>();
            Func<IQueryable<Person>, IOrderedQueryable<Person>> order = 
                p => p.OrderBy(model => model.Activated)
                    .ThenBy(model => model.Gender)
                    .ThenBy(model => model.IdOcupation)
                    .ThenByDescending(model => model.PersonName);

            var filteredAndOrderedResults = business.Select(
                w => w.Activated,
                order
            );

            Assert.IsNotNull(filteredAndOrderedResults);

            filteredAndOrderedResults = business.SelectAll(
                order
            );

            Assert.IsNotNull(filteredAndOrderedResults);
        }
    }
}
