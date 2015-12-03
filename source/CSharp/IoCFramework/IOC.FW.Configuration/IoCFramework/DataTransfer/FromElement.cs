using IOC.FW.Configuration.IoCFramework.DataTransfer.Abstraction;
using System.Configuration;

namespace IOC.FW.Configuration.IoCFramework.DataTransfer
{
    public class FromElement
        : TransferElementAbstract
    {
        [ConfigurationProperty(ConfigurationVariables.SearchPatternKey, DefaultValue = "*", IsKey = true)]
        public string SearchPatterns
        {
            get
            {
                return (string)this[ConfigurationVariables.SearchPatternKey];
            }
        }

        [ConfigurationProperty(ConfigurationVariables.DeleteAfterKey, DefaultValue = false, IsKey = true)]
        public bool DeleteAfter
        {
            get
            {
                return (bool)this[ConfigurationVariables.DeleteAfterKey];
            }
        }
    }
}