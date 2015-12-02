using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http.Dependencies;

using SimpleInjector;

namespace IOC.FW.Web.MVC.DIContainer.SimpleInjector
{
    public sealed class SimpleInjectorWebApiDependencyResolver
     : IDependencyResolver
    {
        private readonly Container container;

        public SimpleInjectorWebApiDependencyResolver(
            Container container)
        {
            this.container = container;
        }

        [DebuggerStepThrough]
        public IDependencyScope BeginScope()
        {
            return this;
        }

        [DebuggerStepThrough]
        public object GetService(Type serviceType)
        {
            return ((IServiceProvider)container)
                .GetService(serviceType);
        }

        [DebuggerStepThrough]
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.GetAllInstances(serviceType);
        }

        [DebuggerStepThrough]
        public void Dispose()
        {
        }
    }
}