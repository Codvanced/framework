using System;
using System.Collections.Generic;
using System.Linq;
using IOC.Abstraction.Business;
using IOC.FW.Core;
using IOC.FW.Core.Abstraction.Business;
using IOC.FW.Core.Abstraction.DAO;
using IOC.FW.Core.Factory;
using IOC.Model;
using NUnit.Framework;
using SimpleInjector;
using System.Diagnostics;
using System.Linq.Expressions;
using IOC.Validation;

namespace IOC.Test
{
    [TestFixture]
    public class NewsTests
    {
        public static IEnumerable<Container> Configure()
        {
            yield return InstanceFactory.RegisterModules();
        }

        [Test, TestCaseSource("Configure")]
        public void TestQuery(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<NewsBusinessAbstract>();
            Assert.NotNull(business);

            //ar news = business.Select(p => true).Take(2);
            business.Test("");
        }

        [Test, TestCaseSource("Configure")]
        public void InsertTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            //var business = InstanceFactory.GetImplementation<NewsBusinessAbstract>();
            var business = InstanceFactory.GetImplementation<IBaseBusiness<News>>();
            Assert.NotNull(business);

            business.Insert(
                new News
                {
                    Title = "Titulo",
                    Author = "Autor",
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    NewsDescription = "Alguma descrição"
                }
            );

            var foundNews = business.SelectSingle(
                n =>
                   n.Title == "Titulo"
                && n.NewsDescription == "Alguma descrição"
                && n.Author == "Autor"
            );
            Assert.NotNull(foundNews);
            Assert.Greater(foundNews.IdNews, 0);
        }

        [Test, TestCaseSource("Configure")]
        public void UpdateTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<NewsBusinessAbstract>();
            Assert.NotNull(business);

            var foundNews = business.SelectAll();
            Assert.NotNull(foundNews);
            Assert.Greater(foundNews.Count, 0);

            foundNews[0].Author = "Modified 1";
            foundNews[0].Updated = DateTime.Now;

            business.Update(foundNews[0]);

            var idNews = foundNews[0].IdNews;
            var updatedNews = business.SelectSingle(
                n => n.IdNews == idNews
            );

            updatedNews.Author = "Modified 2";

            Expression<Func<News, object>>[] expr = { n => n.Author, n => n.Updated };
            business.Update(
                updatedNews,
                expr
            );

            Assert.NotNull(updatedNews);
            Assert.AreEqual(updatedNews.Author, foundNews[0].Author);
        }

        [Test, TestCaseSource("Configure")]
        public void DeleteTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<NewsBusinessAbstract>();
            Assert.NotNull(business);

            var foundNews = business.SelectAll();
            Assert.NotNull(foundNews);
            Assert.Greater(foundNews.Count, 0);

            business.Delete(foundNews[0]);
            var updatedNews = business.SelectSingle(
                n => n.IdNews == foundNews[0].IdNews
            );

            Assert.IsNull(updatedNews);
        }

        [Test, TestCaseSource("Configure")]
        public void TitleRule(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<NewsBusinessAbstract>();
            Assert.NotNull(business);

            business.Insert(
                new News
                {
                    Title = "Titulo",
                    Author = "Autor",
                    NewsDescription = "Descrição",
                    Created = DateTime.Now,
                    Updated = DateTime.Now
                }
            );

            bool titleExists = business.TitleAlreadyExists("Titulo");
            var existsNewsTitle = business.SelectSingle(n => n.Title == "Titulo");

            Assert.NotNull(existsNewsTitle);
            Assert.Greater(existsNewsTitle.IdNews, 0);

            titleExists = business.TitleAlreadyExists("Titulo2", existsNewsTitle.IdNews);
            existsNewsTitle = business.SelectSingle(n => n.Title == "Titulo2");

            Assert.Null(existsNewsTitle);
        }

        [Test, TestCaseSource("Configure")]
        public void TesteTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            //Teste validando título
            var business = InstanceFactory.GetImplementation<NewsBusinessAbstract>();
            Assert.NotNull(business);

            var result = business.TitleAlreadyExists("Titulo");
            Assert.NotNull(result);

            //Teste validando o Execute Scalar
            var dao = InstanceFactory.GetImplementation<IBaseDAO<News>>();
            Assert.NotNull(dao);

            var t = dao.ExecuteQuery(
                "SELECT * FROM News", 
                parameters: null,
                cmdType: System.Data.CommandType.Text
            );

            string query = "SELECT COUNT(0) FROM News";
            var execScalar = dao.ExecuteScalar(query);
            Assert.NotNull(execScalar);
            Assert.IsInstanceOf(typeof(int), execScalar);
        }

        [Test, TestCaseSource("Configure")]
        public void ValidationTest(Container simpleInjector)
        {
            News badNews = new News();

            NewsValidation validation = new NewsValidation();
            var result = validation.Validate(badNews);
            Assert.AreEqual(result.IsValid, false);

            News goodNews = new News();
            goodNews.Title = "teste";
            goodNews.Author = "teste";
            goodNews.NewsDescription = "teste";
            goodNews.NewsDate = DateTime.Now;
            goodNews.Created = DateTime.Now;

            result = validation.Validate(goodNews);
            Assert.AreEqual(result.IsValid, true);
        }

        [Test]
        public void Teste()
        {
            string pattern = "este1".PreparePattern(' ', ".*");
            Assert.IsNotEmpty(pattern);

            List<string> listDynamic = new List<string>() { 
                "TESTE1",
                "TESTE2",
                "TESTE1",
                "TESTE4",
                "TESTE1"
            };

            var newsFound =
                (
                    from
                        search
                        in listDynamic
                    where
                        search.Like(pattern)
                    select search
                ).ToList();
            Assert.IsNotEmpty(newsFound);
        }

        [Test, TestCaseSource("Configure")]
        public void SelectOrderBy(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);
            var business = InstanceFactory.GetImplementation<IBaseBusiness<News>>();
            
            var result = business.Select(
                where: w => w.IdNews > 5,
                navigationProperties: null
            );
        }
    }
}