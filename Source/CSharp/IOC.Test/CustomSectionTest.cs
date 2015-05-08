using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Configuration;
using IOC.FW.Configuration;
using System.Collections.Specialized;
using IOC.FW.Core.Abstraction.Binding;
using SimpleInjector;
using IOC.FW.Core.Factory;
using IOC.Abstraction.Business;

namespace IOC.Test
{
    [TestFixture]
    public class CustomSectionTest
    {
        [Test]
        public void InjectionFactoryTest()
        {
            var container = new Container();
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
                        var module = (IModule)instance.Unwrap();

                        if (module is IModule)
                        {
                            module.SetBinding(container);
                        }
                    }
                }

                var business = InstanceFactory.GetImplementation<NewsBusinessAbstract>();
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
