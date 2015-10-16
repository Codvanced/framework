using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOC.Abstraction.DAO;
using IOC.FW.Core;
using IOC.FW.Core.Abstraction.DAO;
using IOC.FW.Core.Database.Repository;
using IOC.FW.Core.Factory;
using IOC.Model;
using Moq;
using NUnit.Framework;

namespace IOC.Test.DAO
{
    [TestFixture]
    public class IRepositoryTest
    {
        [TestCase]
        public void Should_Do_Insert_Test()
        {
            //SetUp
            int identityScope = 1;
            var newsStorage = new List<Ocupation>();
            var newsInserted = new List<Ocupation>();

            var mockSet = new Mock<DbSet<Ocupation>>();
            var mockContext = new Mock<IContext<Ocupation>>();
            var mockContextFactory = new Mock<IContextFactory<Ocupation>>();
            
            mockContextFactory
                .Setup(
                    m => m.GetContext(It.IsAny<string>())
                )
                .Returns(
                    mockContext.Object
                );
            
            mockContext
                .Setup(
                    m => m.DbObject
                )
                .Returns(
                    mockSet.Object
                );

            mockContext
                .Setup(
                    m => m.SetState(It.IsAny<Ocupation>(), It.IsAny<EntityState>())
                )
                .Callback<Ocupation, EntityState>(
                    (entity, state) =>
                    {
                        newsInserted.Add(entity);
                    }
                );

            mockContext
                .Setup(m => m.SaveChanges())
                .Callback(
                    () =>
                    {
                        newsStorage.AddRange(newsInserted);
                        newsInserted.ForEach(
                            n =>
                            {
                                n.IdOcupation = identityScope++;
                            }
                        );
                    }
                );

            var newsToInsert = new Ocupation[] 
            { 
                new Ocupation
                {
                    OcupationName = "Mock Test 1"
                },
                new Ocupation
                {
                    OcupationName = "Mock Test 2"
                },
                new Ocupation
                {
                    OcupationName = "Mock Test 3"
                }
            };

            //Action
            var repository = new EntityFrameworkRepository<Ocupation>(mockContextFactory.Object);
            repository.Insert(newsToInsert);

            //Assert
            mockContext.Verify(
                m => m.SetState(It.IsAny<Ocupation>(), It.IsAny<EntityState>()),
                Times.Exactly(newsToInsert.Length)
            );
            
            mockContext.Verify(m => m.SaveChanges(), Times.Once());

            Assert.IsNotEmpty(newsStorage);
            Assert.Greater(newsStorage[0].Created, DateTime.MinValue);
            Assert.Greater(newsStorage[1].Created, DateTime.MinValue);
            Assert.Greater(newsStorage[2].Created, DateTime.MinValue);
            Assert.IsTrue(newsStorage[0].Activated);
            Assert.IsTrue(newsStorage[1].Activated);
            Assert.IsTrue(newsStorage[2].Activated);
        }
    }
}