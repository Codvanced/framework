using System;
using System.Collections.Generic;
using IOC.FW.Core.Abstraction.Business;
using IOC.FW.Core.Abstraction.DIContainer;
using IOC.FW.Core.Abstraction.DIContainer.Binding;
using IOC.FW.Core.Abstraction.Repository;
using IOC.FW.Core.Implementation.Base;
using IOC.FW.Core.Implementation.Repository;
using IOC.FW.Core.Implementation.Repository.ADO;
using IOC.FW.Core.Implementation.Repository.EntityFramework;

namespace IOC.FW.Core.Implementation.DIContainer.Binding
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
                typeof(IBaseBusiness<>),
                typeof(BaseBusiness<>),
                Enumerators.LifeCycleType.Singleton
            );

            adapter.Register(
                typeof(IContextFactory<>),
                typeof(ContextFactory<>),
                Enumerators.LifeCycleType.Singleton
            );

            adapter.Register(
                typeof(IRepositoryFactory<>),
                typeof(RepositoryFactory<>),
                Enumerators.LifeCycleType.Singleton
            );

            adapter.RegisterMany(
                typeof(IRepository<>),
                new Dictionary<object, Type>
                {
                    { Enumerators.RepositoryType.EntityFramework, typeof(EntityFrameworkRepository<>) },
                    { Enumerators.RepositoryType.ADO, typeof(ADORepository<>) }
                },
                Enumerators.LifeCycleType.Transient
            );
        }
    }
}