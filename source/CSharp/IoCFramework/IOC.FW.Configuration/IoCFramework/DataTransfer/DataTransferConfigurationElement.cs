using System.Configuration;

namespace IOC.FW.Configuration.IoCFramework.DataTransfer
{
    public class DataTransferConfigurationElement
        : ConfigurationElement
    {
        [ConfigurationProperty(ConfigurationVariables.TransfersKey)]
        [ConfigurationCollection(typeof(TransferElementCollection), AddItemName = ConfigurationVariables.TransferKey)]
        public virtual TransferElementCollection Transfers
        {
            get
            {
                return (TransferElementCollection)base[ConfigurationVariables.TransfersKey];
            }
        }
    }
}