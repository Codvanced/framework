using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleInjector;

namespace IOC.FW.Core.Abstraction.DIContainer.Binding
{
    /// <summary>
    /// Abstração para implementação dos bindings.
    /// </summary>
    public interface IBinding
    {
        /// <summary>
        /// Configura a classe concreta que fará a implementação de uma abstração.
        /// </summary>
        /// <param name="adapter">Container ou kernel da injeção de dependência que conterá todos os bindings.</param>
        void SetBinding(IAdapter adapter);
    }
}