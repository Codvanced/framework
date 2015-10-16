using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
    
using SimpleInjector;
using SimpleInjector.Extensions;

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
            return ((IServiceProvider)this.container)
                .GetService(serviceType);
        }

        [DebuggerStepThrough]
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.container.GetAllInstances(serviceType);
        }

        [DebuggerStepThrough]
        public void Dispose()
        {
        }
    }
}