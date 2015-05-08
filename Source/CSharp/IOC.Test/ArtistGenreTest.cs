using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SimpleInjector;
using IOC.FW.Core.Factory;
using IOC.FW.Core.Abstraction.Business;
using IOC.Model;

namespace IOC.Test
{
    [TestFixture]
    public class ArtistGenreTest
    {
        public static IEnumerable<Container> Configure()
        {
            yield return InstanceFactory.RegisterModules();
        }

        [Test, TestCaseSource("Configure")]
        public void InsertTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<IBaseBusiness<ArtistGenre>>();
            Assert.NotNull(business);

            string nameGuid = string.Format("Genre-{0}", Guid.NewGuid().GetType());
            business.Insert(new ArtistGenre
            {
                IdArtist = 1,
                IdGenre = 1
            });

            var foundArtistGenre = business.SelectSingle(artistGenre =>
                artistGenre.IdArtist == 1 
                && artistGenre.IdGenre == 1
            );

            Assert.NotNull(foundArtistGenre);
            Assert.Greater(foundArtistGenre.IdGenre, 0);
        }

        [Test, TestCaseSource("Configure")]
        public void UpdateTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<IBaseBusiness<ArtistGenre>>();
            Assert.NotNull(business);

            var foundArtistGenre = business.SelectAll();
            Assert.NotNull(foundArtistGenre);
            Assert.Greater(foundArtistGenre.Count, 0);

            foundArtistGenre.Last().IdArtist = 2;

            business.Update(foundArtistGenre.Last());
            var updatedGenre = business.SelectSingle(
                n => n.IdGenre == foundArtistGenre.Last().IdGenre
            );

            Assert.NotNull(updatedGenre);
            Assert.AreEqual(updatedGenre.IdGenre, foundArtistGenre.Last().IdGenre);
        }

        [Test, TestCaseSource("Configure")]
        public void DeleteTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<IBaseBusiness<ArtistGenre>>();
            Assert.NotNull(business);

            var foundArtistGenre = business.SelectAll();
            Assert.NotNull(foundArtistGenre);
            Assert.Greater(foundArtistGenre.Count, 0);

            business.Delete(foundArtistGenre.Last());
            var updatedArtistGenre = business.SelectSingle(
                n => 
                    n.IdGenre == foundArtistGenre.Last().IdGenre
                    && n.IdArtist == foundArtistGenre.Last().IdArtist
            );

            Assert.IsNull(updatedArtistGenre);
        }
    }
}
