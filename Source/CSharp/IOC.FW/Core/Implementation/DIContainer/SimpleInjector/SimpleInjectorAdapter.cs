using System;
using System.Collections.Generic;
using IOC.FW.Core.Abstraction.DIContainer;
using SimpleInjector;

namespace IOC.FW.Core.Implementation.DIContainer.SimpleInjector
{
    public class SimpleInjectorAdapter
        : IAdapter, IAdapterVerifiable
    {
        public readonly Container _container;

        public SimpleInjectorAdapter()
        {
            _container = new Container();
        }

        private Lifestyle GetLifeCycle(Enumerators.LifeCycleType lifeCycle)
        {
            var lifeStyle = Lifestyle.Transient;

            switch (lifeCycle)
            {
                case Enumerators.LifeCycleType.Transient:
                    lifeStyle = Lifestyle.Transient;
                    break;
                case Enumerators.LifeCycleType.Singleton:
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
            Enumerators.LifeCycleType lifeCycle
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

        public void Register<TService>(Enumerators.LifeCycleType lifeCycle)
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

        public void Register<TService, TImplementation>(Enumerators.LifeCycleType lifeCycle)
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
            Enumerators.LifeCycleType lifeCycle
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
            return _container.GetInstance(service);
        }

        public object Resolve(
            Type service,
            object serviceKey
        )
        {
            //TODO: Implement serviceKey
            return null;
        }

        public TService Resolve<TService>()
            where TService : class
        {
            return _container.GetInstance<TService>();
        }

        public TService Resolve<TService>(object serviceKey)
            where TService : class
        {
            //TODO: Implement serviceKey
            return null;
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