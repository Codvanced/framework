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
    public class ArtistTest
    {
        public static IEnumerable<Container> Configure()
        {
            yield return InstanceFactory.RegisterModules();
        }

        [Test, TestCaseSource("Configure")]
        public void InsertTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<IBaseBusiness<Artist>>();
            Assert.NotNull(business);

            string nameGuid = string.Format("Artist-{0}", Guid.NewGuid().GetType());
            business.Insert(new Artist
            {
                Name = nameGuid
            });

            var foundArtist = business.SelectSingle(artist =>
                artist.Name == nameGuid
            );

            Assert.NotNull(foundArtist);
            Assert.Greater(foundArtist.IdArtist, 0);
        }

        [Test, TestCaseSource("Configure")]
        public void UpdateTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<IBaseBusiness<Artist>>();
            Assert.NotNull(business);

            var foundArtist = business.SelectAll();
            Assert.NotNull(foundArtist);
            Assert.Greater(foundArtist.Count, 0);

            foundArtist.Last().Name = "Modified";

            business.Update(foundArtist.Last());
            var updatedNews = business.SelectSingle(
                n => n.IdArtist == foundArtist.Last().IdArtist
            );

            Assert.NotNull(updatedNews);
            Assert.AreEqual(updatedNews.IdArtist, foundArtist.Last().IdArtist);
        }

        [Test, TestCaseSource("Configure")]
        public void DeleteTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<IBaseBusiness<Artist>>();
            Assert.NotNull(business);

            var foundArtist = business.SelectAll();
            Assert.NotNull(foundArtist);
            Assert.Greater(foundArtist.Count, 0);

            business.Delete(foundArtist.Last());
            var updatedNews = business.SelectSingle(
                n => n.IdArtist == foundArtist.Last().IdArtist
            );

            Assert.IsNull(updatedNews);
        }
    }
}
