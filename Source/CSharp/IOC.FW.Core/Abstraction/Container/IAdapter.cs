using IOC.FW.Shared.Enumerators;
using System;
using System.Collections.Generic;

namespace IOC.FW.Core.Abstraction.Container
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
            ContainerEnumerator.LifeCycle lifeCycle
        );

        void Register<TService>()
            where TService : class;

        void Register<TService>(
            ContainerEnumerator.LifeCycle lifeCycle
        ) where TService : class;

        void Register<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        void Register<TService, TImplementation>(ContainerEnumerator.LifeCycle lifeCycle)
            where TService : class
            where TImplementation : class, TService;

        void RegisterMany(
            Type service,
            IDictionary<object, Type> implementations
        );

        void RegisterMany(
            Type service,
            IDictionary<object, Type> implementations,
            ContainerEnumerator.LifeCycle lifeCycle
        );

        void RegisterMany<TService>(
            IDictionary<object, Type> implementations
        ) where TService : class;

        void RegisterMany<TService>(
            IDictionary<object, Type> implementations,
            ContainerEnumerator.LifeCycle lifeCycle
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