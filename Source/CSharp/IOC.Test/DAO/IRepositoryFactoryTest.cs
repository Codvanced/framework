using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.FW.Core;
using IOC.FW.Core.Abstraction.DAO;
using IOC.FW.Core.Factory;
using IOC.Model;
using NUnit.Framework;

namespace IOC.Test.DAO
{
    [TestFixture]
    public class IRepositoryFactoryTest
    {
        [TestCase]
        public void Should_Get_Repository_Implementation_Of_The_Factory_Test()
        {
            var factory = InstanceFactory.GetImplementation<IRepositoryFactory<Ocupation>>();
            var implementation = factory.GetRepository(Enumerators.RepositoryType.EntityFramework);
            Assert.IsNotNull(implementation);
        }
    }
}