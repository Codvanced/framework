using System;
using System.Collections.Generic;
using System.Linq;
using IOC.Abstraction.Business;
using IOC.FW.Core;
using IOC.FW.Core.Abstraction.Business;
using IOC.FW.Core.Abstraction.DAO;
using IOC.FW.Core.Factory;
using IOC.Model;
using IOC.Validation;
using NUnit.Framework;
using SimpleInjector;
using System.Diagnostics;

namespace IOC.Test
{
    [TestFixture]
    public class NoticiaTests
    {
        public static IEnumerable<Container> Configure()
        {
            yield return InstanceFactory.RegisterModules();
        }

        [Test, TestCaseSource("Configure")]
        public void TestQuery(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<AbstractNoticiaBusiness>();
            Assert.NotNull(business);

            var noticias = business.Select(p => true).Take(2);
        }

        [Test, TestCaseSource("Configure")]
        public void InsertTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<AbstractNoticiaBusiness>();
            Assert.NotNull(business);

            business.Insert(new Noticia
            {
                Autor = "Autor",
                DataCadastro = DateTime.Now,
                DataNoticia = DateTime.Now,
                Descricao = "Alguma descrição",
                Titulo = "Titulo"
            });

            var foundNoticia = business.SelectSingle(noticia =>
                noticia.Titulo == "Titulo"
                && noticia.Descricao == "Alguma descrição"
                && noticia.Autor == "Autor"
            );
            Assert.NotNull(foundNoticia);
            Assert.Greater(foundNoticia.IdNoticia, 0);
        }

        [Test, TestCaseSource("Configure")]
        public void UpdateTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<AbstractNoticiaBusiness>();
            Assert.NotNull(business);

            var foundNoticia = business.SelectAll();
            Assert.NotNull(foundNoticia);
            Assert.Greater(foundNoticia.Count, 0);

            foundNoticia[0].Autor = "Modified";
            foundNoticia[0].DataAlteracao = DateTime.Now;

            business.Update(foundNoticia[0]);
            var updatedNoticia = business.SelectSingle(
                noticia => noticia.IdNoticia == foundNoticia[0].IdNoticia
            );

            Assert.NotNull(updatedNoticia);
            Assert.AreEqual(updatedNoticia.Autor, foundNoticia[0].Autor);
        }

        [Test, TestCaseSource("Configure")]
        public void DeleteTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<AbstractNoticiaBusiness>();
            Assert.NotNull(business);

            var foundNoticia = business.SelectAll();
            Assert.NotNull(foundNoticia);
            Assert.Greater(foundNoticia.Count, 0);

            business.Delete(foundNoticia[0]);
            var updatedNoticia = business.SelectSingle(
                noticia => noticia.IdNoticia == foundNoticia[0].IdNoticia
            );

            Assert.IsNull(updatedNoticia);
        }

        [Test, TestCaseSource("Configure")]
        public void TitleRule(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<AbstractNoticiaBusiness>();
            Assert.NotNull(business);

            business.Insert(new Noticia
            {
                Autor = "Autor",
                Descricao = "Descrição",
                Titulo = "Titulo",
                DataNoticia = DateTime.Now,
                DataCadastro = DateTime.Now
            });

            bool titleExists = business.TitleAlreadyExists("Titulo");
            var existsNewsTitle = business.SelectSingle(noticia => noticia.Titulo == "Titulo");

            Assert.NotNull(existsNewsTitle);
            Assert.Greater(existsNewsTitle.IdNoticia, 0);

            titleExists = business.TitleAlreadyExists("Titulo2", existsNewsTitle.IdNoticia);
            existsNewsTitle = business.SelectSingle(noticia => noticia.Titulo == "Titulo2");

            Assert.Null(existsNewsTitle);
        }

        [Test, TestCaseSource("Configure")]
        public void TesteTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            //Teste validando título
            var business = InstanceFactory.GetImplementation<AbstractNoticiaBusiness>();
            Assert.NotNull(business);

            var result = business.TitleAlreadyExists("Titulo");
            Assert.NotNull(result);

            //Teste validando o Execute Scalar
            var dao = InstanceFactory.GetImplementation<IBaseDAO<Noticia>>();
            Assert.NotNull(dao);

            var t = dao.ExecuteQuery("SELECT * FROM WT_NOTICIA", parameters: null, cmdType: System.Data.CommandType.Text);

            string query = "SELECT COUNT(0) FROM WT_NOTICIA";
            var execScalar = dao.ExecuteScalar(query);
            Assert.NotNull(execScalar);
            Assert.IsInstanceOf(typeof(int), execScalar);
        }

        [Test, TestCaseSource("Configure")]
        public void ValidationTest(Container simpleInjector)
        {
            Noticia badNoticia = new Noticia();
            
            NoticiaValidation validation = new NoticiaValidation();
            var result = validation.Validate(badNoticia);
            Assert.AreEqual(result.IsValid, false);

            Noticia goodNoticia = new Noticia();
            goodNoticia.DataNoticia = DateTime.Now;
            goodNoticia.DataCadastro = DateTime.Now;
            goodNoticia.Autor = "teste";
            goodNoticia.Descricao = "teste";
            goodNoticia.Titulo = "teste";

            result = validation.Validate(goodNoticia);
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
        public void BulkInsertTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<IBaseBusiness<Noticia>>();
            Assert.NotNull(business);

            var watch = Stopwatch.StartNew();

            var length = 50;
            var models = new Noticia[length];
            Assert.NotNull(models);

            for (var i = 0; i < length; i++)
                models[i] = new Noticia()
                {
                    Autor = "Autor",
                    Descricao = "Descrição",
                    Titulo = "Titulo",
                    DataNoticia = DateTime.Now,
                    DataCadastro = DateTime.Now
                };

            business.Insert(models);

            watch.Stop();

            var time = watch.ElapsedMilliseconds;
        }

        [Test, TestCaseSource("Configure")]
        public void BulkInsertTransactionTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<IBaseBusiness<Noticia>>();
            Assert.NotNull(business);

            var watch = Stopwatch.StartNew();

            var length = 50;
            var models = new Noticia[length];
            Assert.NotNull(models);

            for (var i = 0; i < length; i++)
                models[i] = new Noticia()
                {
                    Autor = "Autor",
                    Descricao = "Descrição",
                    Titulo = "Titulo",
                    DataNoticia = DateTime.Now,
                    DataCadastro = DateTime.Now
                };

            business.BulkInsert(models);

            watch.Stop();

            var time = watch.ElapsedMilliseconds;
        }
    }
}