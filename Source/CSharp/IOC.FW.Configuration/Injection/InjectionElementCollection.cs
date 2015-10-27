using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace IOC.FW.Configuration.Injection
{
    /// <summary>
    /// Classe de implementação do elemento injection do customSection
    /// </summary>
    public class InjectionElementCollection
         : ConfigurationElementCollection
    {
        /// <summary>
        /// Propriadade representa um elemento da coleção do injection.
        /// </summary>
        /// <param name="key">Chave de um elemento da coleção do injection.</param>
        /// <returns>Valor de um elemento da coleção injection.</returns>
        public new InjectionElement this[string key]
        {
            get
            {
                return (InjectionElement)base.BaseGet(key);
            }
        }

        /// <summary>
        /// Propriadade representa um elemento da coleção injection.
        /// </summary>
        /// <param name="index">Índice de um elemento da coleção injection.</param>
        /// <returns>Valor de um elemento da coleção injection.</returns>
        public InjectionElement this[int index]
        {
            get
            {
                return (InjectionElement)base.BaseGet(index);
            }
        }

        /// <summary>
        /// Instância um elemento da coleção injection.
        /// </summary>
        /// <returns>Retorna a instância de um elemento da coleção injection.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new InjectionElement();
        }

        /// <summary>
        /// Pega uma instância da chave de um elemento da coleção injection.
        /// </summary>
        /// <param name="element">Uma instância ou implementação de ConfigurationElement.</param>
        /// <returns>Retorna o objecto que representa a chave do elemento do customSection.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((InjectionElement)element).Key;
        }
    }
}