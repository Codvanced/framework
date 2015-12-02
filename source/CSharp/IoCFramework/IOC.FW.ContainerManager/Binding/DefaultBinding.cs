using System;
using System.Collections.Generic;
using IOC.FW.Core.Abstraction.Business;
using IOC.FW.Core.Abstraction.Repository;
using IOC.FW.Core.Implementation.Base;
using IOC.FW.Core.Abstraction.Container.Binding;
using IOC.FW.Core.Abstraction.Container;
using IOC.FW.Repository;
using IOC.FW.Repository.EntityFramework;
using IOC.FW.Repository.ADO;
using IOC.FW.Shared.Enumerators;

namespace IOC.FW.ContainerManager.Binding
{
    internal class DefaultBinding
        : IBinding
    {
        /// <summary>
        /// Método destinado a settar os bindings padrões
        /// </summary>
        /// <param name="container">Container de injeção (Simple Injector)</param>
        public void SetBinding(IAdapter adapter)
        {
            adapter.Register(
                typeof(IAdapter),
                adapter.GetType(),
                ContainerEnumerator.LifeCycle.Singleton
            );

            adapter.Register(
                typeof(IBaseBusiness<>),
                typeof(BaseBusiness<>),
                ContainerEnumerator.LifeCycle.Singleton
            );

            adapter.Register(
                typeof(IContextFactory<>),
                typeof(ContextFactory<>),
                ContainerEnumerator.LifeCycle.Singleton
            );

            adapter.Register(
                typeof(IRepositoryFactory<>),
                typeof(RepositoryFactory<>),
                ContainerEnumerator.LifeCycle.Singleton
            );

            adapter.RegisterMany(
                typeof(IRepository<>),
                new Dictionary<object, Type>
                {
                    { RepositoryEnumerator.RepositoryType.EntityFramework, typeof(EntityFrameworkRepository<>) },
                    { RepositoryEnumerator.RepositoryType.ADO, typeof(ADORepository<>) }
                },
                ContainerEnumerator.LifeCycle.Transient
            );
        }
    }
}