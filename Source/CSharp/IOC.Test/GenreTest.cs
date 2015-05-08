using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SimpleInjector;
using IOC.FW.Core.Factory;
using IOC.Model;
using IOC.FW.Core.Abstraction.Business;

namespace IOC.Test
{
    [TestFixture]
    public class GenreTest
    {
        public static IEnumerable<Container> Configure()
        {
            yield return InstanceFactory.RegisterModules();
        }

        [Test, TestCaseSource("Configure")]
        public void InsertTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<IBaseBusiness<Genre>>();
            Assert.NotNull(business);

            string nameGuid = string.Format("Genre-{0}", Guid.NewGuid().GetType());
            business.Insert(new Genre
            {
                Name = nameGuid
            });

            var foundGenre = business.SelectSingle(artist =>
                artist.Name == nameGuid
            );
            Assert.NotNull(foundGenre);
            Assert.Greater(foundGenre.IdGenre, 0);
        }

        [Test, TestCaseSource("Configure")]
        public void UpdateTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<IBaseBusiness<Genre>>();
            Assert.NotNull(business);

            var foundGenre = business.SelectAll();
            Assert.NotNull(foundGenre);
            Assert.Greater(foundGenre.Count, 0);

            foundGenre.Last().Name = "Modified";

            business.Update(foundGenre.Last());
            var updatedGenre = business.SelectSingle(
                n => n.IdGenre == foundGenre.Last().IdGenre
            );

            Assert.NotNull(updatedGenre);
            Assert.AreEqual(updatedGenre.IdGenre, foundGenre.Last().IdGenre);
        }

        [Test, TestCaseSource("Configure")]
        public void DeleteTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<IBaseBusiness<Genre>>();
            Assert.NotNull(business);

            var foundGenre = business.SelectAll();
            Assert.NotNull(foundGenre);
            Assert.Greater(foundGenre.Count, 0);

            business.Delete(foundGenre.Last());
            var updatedGenre = business.SelectSingle(
                n => n.IdGenre == foundGenre.Last().IdGenre
            );

            Assert.IsNull(updatedGenre);
        }
    }
}
