using System.Configuration;

namespace IOC.FW.Configuration.Injection
{
    /// <summary>
    /// Classe de configuração do elemento injectionFactory do customSection.
    /// </summary>
    public class InjectionFactoryConfigurationElement
        : ConfigurationElement
    {
        /// <summary>
        /// Propriedade representa o elemento injection do customSection e comtém a coleção de elementos e suas propriedades.
        /// </summary>
        [ConfigurationProperty("injection", IsRequired = true)]
        public InjectionElementCollection Injection
        {
            get
            {
                return (InjectionElementCollection)this["injection"];
            }
        }
    }
}