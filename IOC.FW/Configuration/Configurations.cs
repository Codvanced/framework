using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using IOC.FW.Configuration.Injection;
using IOC.FW.Configuration.Thumb;

namespace IOC.FW.Configuration
{
    /// <summary>
    /// Classe de implementação do customSection.
    /// </summary>
    public class Configurations
        : ConfigurationSection
    {
        /// <summary>
        /// Propriedade representa os elementos do customSection com as configurações atuais.
        /// </summary>
        public static Configurations Current
        {
            get
            {
                return ConfigurationManager.GetSection("customSection") as Configurations;
            }
        }

        /// <summary>
        /// Propriedade representa o elemento injectionFactory do customSection e comtém suas propriedades.
        /// </summary>
        [ConfigurationProperty("injectionFactory", IsRequired = false)]
        public InjectionFactoryConfigurationElement InjectionFactory
        {
            get
            {
                return (InjectionFactoryConfigurationElement)this["injectionFactory"];
            }
        }

        /// <summary>
        /// Propriedade representa o elemento thumb do customSection e comtém suas propriedades.
        /// </summary>
        [ConfigurationProperty("thumb", IsRequired = false)]
        public ThumbElement Thumb
        {
            get
            {
                return (ThumbElement)this["thumb"];
            }
        }
    }
}