using System;
using System.Collections.Generic;
using SimpleInjector;
using IOC.FW.Core.Abstraction.Container;
using IOC.FW.Shared.Enumerators;

namespace IOC.FW.ContainerManager.SimpleInjector
{
    public class SimpleInjectorAdapter
        : IAdapter, IAdapterVerifiable
    {
        public readonly Container _container;

        public SimpleInjectorAdapter()
        {
            _container = new Container();
        }

        private Lifestyle GetLifeCycle(ContainerEnumerator.LifeCycle lifeCycle)
        {
            var lifeStyle = Lifestyle.Transient;

            switch (lifeCycle)
            {
                case ContainerEnumerator.LifeCycle.Transient:
                    lifeStyle = Lifestyle.Transient;
                    break;
                case ContainerEnumerator.LifeCycle.Singleton:
                    lifeStyle = Lifestyle.Singleton;
                    break;
                default:
                    lifeStyle = Lifestyle.Transient;
                    break;
            }

            return lifeStyle;
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
            var lifeStyle = GetLifeCycle(lifeCycle);
            _container.Register(service, implementation, lifeStyle);
        }

        public void Register<TService>()
            where TService : class
        {
            _container.Register<TService>();
        }

        public void Register<TService>(ContainerEnumerator.LifeCycle lifeCycle)
            where TService : class
        {
            var lifeStyle = GetLifeCycle(lifeCycle);
            _container.Register<TService>(lifeStyle);
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
            var lifeStyle = GetLifeCycle(lifeCycle);
            _container.Register<TService, TImplementation>(lifeStyle);
        }

        public void RegisterMany(
            Type service,
            IDictionary<object, Type> implementations
        )
        {
            _container.RegisterCollection(service, implementations.Values);
        }

        public void RegisterMany(
            Type service,
            IDictionary<object, Type> implementations,
            ContainerEnumerator.LifeCycle lifeCycle
        )
        {
            RegisterMany(service, implementations);
        }

        public void RegisterMany<TService>(IDictionary<object, Type> implementations)
            where TService : class
        {
            _container.RegisterCollection<TService>(implementations.Values);
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
            return _container.GetInstance(service);
        }

        public TService Resolve<TService>()
            where TService : class
        {
            return _container.GetInstance<TService>();
        }

        public object Resolve(
            Type service,
            object serviceKey
        )
        {
            throw new NotImplementedException();
        }

        public TService Resolve<TService>(object serviceKey)
            where TService : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TService> ResolveMany<TService>()
            where TService : class
        {
            return _container.GetAllInstances<TService>();
        }

        public void Verify()
        {
            _container.Verify(VerificationOption.VerifyAndDiagnose);
        }
    }
}