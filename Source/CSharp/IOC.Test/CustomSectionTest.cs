using System;
using NUnit.Framework;
using IOC.FW.Configuration;
using IOC.Abstraction.Business;
using IOC.FW.ContainerManager.SimpleInjector;
using IOC.FW.Core.Abstraction.Container.Binding;

namespace IOC.Test
{
    [TestFixture]
    public class CustomSectionTest
    {
        [Test]
        public void ContainerManagerTest()
        {
            var adapter = new SimpleInjectorAdapter();
            var configuration = ConfigManager.GetConfig();

            foreach (var module in configuration.ContainerManager.Modules)
            {
                var instance = Activator.CreateInstance(
                    module.AssemblyName,
                    module.ClassName
                );
                var instanceModule = (IBinding)instance.Unwrap();

                if (instanceModule is IBinding)
                {
                    instanceModule.SetBinding(adapter);
                }
            }

            var business = adapter.Resolve<INewsBusiness>();
            Assert.IsNotNull(business);
        }

        [Test]
        public void ThumbTest() 
        {
            var configuration = ConfigManager.GetConfig();
            var thumb = configuration.Thumb;
            var notFoundPath = thumb.NotFoundPath;
            var defaultWidth = thumb.DefaultWidth;
            var defaultHeiht = thumb.DefaultHeight;

            Assert.NotNull(thumb);
            Assert.NotNull(notFoundPath);
            Assert.NotNull(defaultWidth);
            Assert.NotNull(defaultHeiht);
        }
    }
}