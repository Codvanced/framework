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
using Moq;

namespace IOC.Test
{
    [TestFixture]
    public class IBaseBusinessTest
    {
        public static IEnumerable<Container> Configure()
        {
            yield return InstanceFactory.RegisterModules();
        }

        [Test]
        public void BaseTest(Container simpleInjector)
        {
            var business = InstanceFactory.GetImplementation<IBaseBusiness<Person>>();
            var instanceModel = business.Model();
            var instanceListModel = business.List();
        }

        [Test]
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

            filteredAndOrderedResults = null;
            Assert.IsNotNull(filteredAndOrderedResults);

            filteredAndOrderedResults = business.SelectAll(
                order
            );

            Assert.IsNotNull(filteredAndOrderedResults);
        }

        public void create_model_test()
        {
            var business = InstanceFactory.GetImplementation<IBaseBusiness<Person>>();
            Assert.IsNotNull(business);
            
            var model = business.Model();
            Assert.IsNotNull(model);
        }

        public void create_list_model_test()
        {
            var business = InstanceFactory.GetImplementation<IBaseBusiness<Person>>();
            Assert.IsNotNull(business);

            var model = business.List();
            Assert.IsNotNull(model);
        }

        public void insert_test()
        {
            var business = InstanceFactory.GetImplementation<IBaseBusiness<Person>>();
            var model = business.Model();
            model.Activated = (new Random().Next(0, 1)) == 1;
            model.Created = DateTime.Now;
            model.Gender = (new Random().Next(0, 1)) == 1 ? "M" : "F";
            NUnit.Framework.Randomizer.GetRandomizer(model.PersonName.GetType());
        }

    }
}
