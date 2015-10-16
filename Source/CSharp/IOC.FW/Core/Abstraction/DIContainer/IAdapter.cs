using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOC.FW.Core.Abstraction.DIContainer
{
    public interface IAdapter
    {
        void Register(
            Type service,
            Type implementation
        );

        void Register(
            Type service,
            Type implementation,
            Enumerators.LifeCycleType lifeCycle
        );

        void Register<TService>()
            where TService : class;

        void Register<TService>(
            Enumerators.LifeCycleType lifeCycle
        ) where TService : class;

        void Register<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        void Register<TService, TImplementation>(Enumerators.LifeCycleType lifeCycle)
            where TService : class
            where TImplementation : class, TService;

        void RegisterMany(
            Type service,
            IDictionary<object, Type> implementations
        );

        void RegisterMany(
            Type service,
            IDictionary<object, Type> implementations,
            Enumerators.LifeCycleType lifeCycle
        );

        void RegisterMany<TService>(
            IDictionary<object, Type> implementations
        ) where TService : class;

        void RegisterMany<TService>(
            IDictionary<object, Type> implementations,
            Enumerators.LifeCycleType lifeCycle
        ) where TService : class;

        object Resolve(Type service);

        object Resolve(
            Type service,
            object serviceKey
        );

        TService Resolve<TService>()
            where TService : class;

        TService Resolve<TService>(object serviceKey)
            where TService : class;

        IEnumerable<TService> ResolveMany<TService>()
            where TService : class;
    }
}