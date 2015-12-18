using DryIoc;
using IOC.FW.Abstraction.Container;
using IOC.FW.Shared.Enumerators;
using System;
using System.Collections.Generic;

namespace IOC.FW.ContainerManager.DryIoc
{
    public class DryIocAdapter
        : IAdapter
    {
        public readonly Container _container;

        public DryIocAdapter()
        {
            _container = new Container();
        }

        private IReuse GetLifeCycle(ContainerEnumerator.LifeCycle lifeCycle)
        {
            var reuse = Reuse.Transient;

            switch (lifeCycle)
            {
                case ContainerEnumerator.LifeCycle.Transient:
                    reuse = Reuse.Transient;
                    break;
                case ContainerEnumerator.LifeCycle.Singleton:
                    reuse = Reuse.Singleton;
                    break;
                default:
                    reuse = Reuse.Transient;
                    break;
            }

            return reuse;
        }

        public void Register(
            Type service,
            Type implementation
        )
        {
            _container.Register(service, implementation);
        }

        public void Register(
            Type service,
            Type implementation,
            ContainerEnumerator.LifeCycle lifeCycle
        )
        {
            var reuse = GetLifeCycle(lifeCycle);
            _container.Register(service, implementation, reuse);
        }

        public void Register<TService>()
            where TService : class
        {
            _container.Register<TService>();
        }

        public void Register<TService>(ContainerEnumerator.LifeCycle lifeCycle)
            where TService : class
        {
            var reuse = GetLifeCycle(lifeCycle);
            _container.Register<TService>(reuse);
        }

        public void Register<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            _container.Register<TService, TImplementation>();
        }

        public void Register<TService, TImplementation>(ContainerEnumerator.LifeCycle lifeCycle)
            where TService : class
            where TImplementation : class, TService
        {
            var reuse = GetLifeCycle(lifeCycle);
            _container.Register<TService, TImplementation>(reuse);
        }

        public void RegisterMany(
            Type service,
            IDictionary<object, Type> implementations
        )
        {
            foreach (var implementation in implementations)
            {
                _container.Register(
                    service,
                    implementation.Value,
                    serviceKey: implementation.Key
                );
            }
        }

        public void RegisterMany(
            Type service,
            IDictionary<object, Type> implementations,
            ContainerEnumerator.LifeCycle lifeCycle
        )
        {
            var reuse = GetLifeCycle(lifeCycle);

            foreach (var implementation in implementations)
            {
                _container.Register(
                    service,
                    implementation.Value,
                    reuse,
                    serviceKey: implementation.Key
                );
            }
        }

        public void RegisterMany<TService>(IDictionary<object, Type> implementations)
            where TService : class
        {
            RegisterMany(
                typeof(TService),
                implementations
            );
        }

        public void RegisterMany<TService>(
            IDictionary<object, Type> implementations,
            ContainerEnumerator.LifeCycle lifeCycle
        ) where TService : class
        {
            RegisterMany(
                typeof(TService),
                implementations,
                lifeCycle
            );
        }

        public object Resolve(Type service)
        {
            return _container.Resolve(service);
        }

        public object Resolve(
            Type service,
            object serviceKey
        )
        {
            return _container.Resolve(service, serviceKey);
        }

        public TService Resolve<TService>()
            where TService : class
        {
            return _container.Resolve<TService>();
        }

        public TService Resolve<TService>(object serviceKey) where TService : class
        {
            return _container.Resolve<TService>(serviceKey);
        }

        public IEnumerable<TService> ResolveMany<TService>()
            where TService : class
        {
            return _container.ResolveMany<TService>();
        }
    }
}