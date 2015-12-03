using System.Configuration;

namespace IOC.FW.Configuration.IoCFramework.ContainerManager
{
    public class ContainerManagerConfigurationElement
        : ConfigurationElement
    {
        [ConfigurationProperty(ConfigurationVariables.ModulesKey, IsRequired = true)]
        [ConfigurationCollection(typeof(ModulesElementCollection), AddItemName = ConfigurationVariables.ModuleKey)]
        public virtual ModulesElementCollection Modules
        {
            get
            {
                return (ModulesElementCollection)this[ConfigurationVariables.ModulesKey];
            }
        }
    }
}