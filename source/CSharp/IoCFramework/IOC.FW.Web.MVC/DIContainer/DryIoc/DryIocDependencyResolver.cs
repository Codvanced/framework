using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Compilation;

namespace IOC.FW.Web.MVC.DIContainer.DryIoc
{
    public class DryIocDependencyResolver
        : IDependencyResolver
    {
        public readonly IResolver _resolver;

        public DryIocDependencyResolver(IContainer container)
        {
            _resolver = container;
            RegisterMvcControllers(container);
        }

        public object GetService(Type serviceType)
        {
            return _resolver.Resolve(serviceType, IfUnresolved.ReturnDefault);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _resolver.ResolveMany<object>(serviceType);
        }

        private void RegisterMvcControllers(IContainer container)
        {
            var referencedAssemblies = BuildManager
                .GetReferencedAssemblies()
                .OfType<Assembly>()
                .Where(
                    a =>
                        !a.IsDynamic
                        && !a.GlobalAssemblyCache
                ).SelectMany(
                    a =>
                        a.GetLoadedTypes()
                );
            
            container.RegisterMany(
                referencedAssemblies.Where(
                    t =>
                        typeof(IController).GetTypeInfo()
                        .IsAssignableFrom(
                            t.GetTypeInfo()
                        )
                ),
                Reuse.InResolutionScope,
                FactoryMethod.ConstructorWithResolvableArguments
            );
        }
    }
}