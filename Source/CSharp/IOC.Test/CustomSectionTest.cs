using System;
using NUnit.Framework;
using IOC.FW.Configuration;
using SimpleInjector;
using IOC.Abstraction.Business;
using IOC.FW.Core.Implementation.DIContainer;
using IOC.FW.Core.Abstraction.DIContainer.Binding;
using IOC.FW.Core.Implementation.DIContainer.SimpleInjector;

namespace IOC.Test
{
    [TestFixture]
    public class CustomSectionTest
    {
        [Test]
        public void InjectionFactoryTest()
        {
            var adapter = new SimpleInjectorAdapter();
            var injectionFactory = Configurations.Current.InjectionFactory.Injection;
            if (injectionFactory != null && injectionFactory.Count > 0)
            {
                for (int i = 0; i < injectionFactory.Count; i++)
                {
                    string assembly = injectionFactory[i].Value;
                    string[] assemblies = assembly.Split(',');

                    if (assemblies != null && assemblies.Length >= 1)
                    {
                        var instance = Activator.CreateInstance(assemblies[0].Trim(), assemblies[1].Trim());
                        var module = (IBinding)instance.Unwrap();

                        if (module is IBinding)
                        {
                            module.SetBinding(adapter);
                        }
                    }
                }

                var business = DependencyResolver.Adapter.Resolve<INewsBusiness>();
                Assert.IsNotNull(business);
            }
        }

        [Test]
        public void ThumbTest() 
        {
            var thumb = Configurations.Current.Thumb;
            var notFoundPath = thumb.NotFoundPath;
            var defaultWidth = thumb.DefaultWidth;
            var defaultHeiht = thumb.DefaultHeight;
        }
    }
}
