using DryIoc;
using IOC.FW.Core.Abstraction.DIContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOC.FW.Core.Implementation.DIContainer.DryIoc
{
    public class DryIocAdapter
        : IAdapter
    {
        public readonly Container _container;

        public DryIocAdapter(
            Rules rules = null,
            IScopeContext scopeContext = null
        )
        {
            _container = new Container(rules, scopeContext);
        }

        public DryIocAdapter(
            Func<Rules, Rules> configure,
            IScopeContext scopeContext = null
        )
        {
            _container = new Container(configure, scopeContext);
        }

        private IReuse GetLifeCycle(Enumerators.LifeCycleType lifeCycle)
        {
            var reuse = Reuse.Transient;

            switch (lifeCycle)
            {
                case Enumerators.LifeCycleType.Transient:
                    reuse = Reuse.Transient;
                    break;
                case Enumerators.LifeCycleType.Singleton:
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
            Enumerators.LifeCycleType lifeCycle
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

        public void Register<TService>(Enumerators.LifeCycleType lifeCycle)
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

        public void Register<TService, TImplementation>(Enumerators.LifeCycleType lifeCycle)
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
            Enumerators.LifeCycleType lifeCycle
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
            Enumerators.LifeCycleType lifeCycle
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