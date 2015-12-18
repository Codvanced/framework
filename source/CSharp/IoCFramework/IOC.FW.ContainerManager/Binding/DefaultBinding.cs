using System;
using System.Collections.Generic;
using IOC.FW.Abstraction.Business;
using IOC.FW.Abstraction.Repository;
using IOC.FW.Abstraction.Container.Binding;
using IOC.FW.Abstraction.Container;
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
        }
    }
}