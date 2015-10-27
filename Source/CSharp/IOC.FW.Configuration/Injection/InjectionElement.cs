using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace IOC.FW.Configuration.Injection
{
    /// <summary>
    /// Classe representa o elemento da coleção injection.
    /// </summary>
    public class InjectionElement
        : ConfigurationElement
    {
        /// <summary>
        /// Propriedade representa a chave do elemento da coleção injection.
        /// </summary>
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get
            {
                return this["key"].ToString();
            }
        }

        /// <summary>
        /// Propriedade representa o valor do elemento da coleção injection.
        /// </summary>
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get
            {
                return this["value"].ToString();
            }
        }
    }
}