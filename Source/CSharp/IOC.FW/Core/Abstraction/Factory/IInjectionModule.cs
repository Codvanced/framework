using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOC.FW.Core.Abstraction.Factory
{
    /// <summary>
    /// Abstração para implementação de Containers de Injeção de Dependência
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IInjectionModule<T>
        where T : class
    {
        /// <summary>
        /// Container utilizado
        /// </summary>
        T container { get; set; }
    }
}