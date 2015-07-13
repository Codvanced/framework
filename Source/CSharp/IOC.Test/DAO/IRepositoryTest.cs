using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOC.FW.Core.Abstraction.DAO;
using IOC.FW.Core.Database.Repository;
using IOC.Model;
using Moq;
using NUnit.Framework;

namespace IOC.Test.DAO
{
    [TestFixture]
    class IRepositoryTest
    {
        int ScopeIdentity = 1;
        IList<News> News = new List<News>();

        void Should_Insert_Test()
        {
            var mockRepository = new Mock<IRepository<News>>();
            mockRepository.Setup(
                m => m.Insert(It.IsAny<News>())
            ).Callback<News[]>(
                (obj) =>
                {
                    foreach (var item in obj)
                    {
                        item.IdNews = ScopeIdentity;
                        News.Add(item);
                        ScopeIdentity++;
                    }
                }
            );

            mockRepository.Setup(m => m.SelectAll()).Returns(News);

            var newsToInsert = new News
            {
                Title = "Mock Test",
                NewsDescription = "Test",
                Author = "Test",
                NewsDate = DateTime.Now
            };
            mockRepository.Object.Insert(newsToInsert);
            mockRepository.Verify(m => m.Insert(newsToInsert), Times.Once());

            Assert.AreNotEqual(0, newsToInsert.IdNews);
        }
    }
}