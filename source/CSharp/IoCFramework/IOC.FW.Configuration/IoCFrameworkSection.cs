using IOC.FW.Configuration.IoCFramework.DataTransfer;
using IOC.FW.Configuration.IoCFramework.Thumb;
using System.Configuration;

namespace IOC.FW.Configuration
{
    public class IoCFrameworkSection
        : ConfigurationSection
    {
        [ConfigurationProperty(ConfigurationVariables.ThumbKey)]
        public virtual ThumbElement Thumb
        {
            get
            {
                return (ThumbElement)this[ConfigurationVariables.ThumbKey];
            }
        }

        [ConfigurationProperty(ConfigurationVariables.DataTransferKey)]
        public DataTransferConfigurationElement DataTransfer
        {
            get
            {
                return (DataTransferConfigurationElement)this[ConfigurationVariables.DataTransferKey];
            }
        }
    }
}