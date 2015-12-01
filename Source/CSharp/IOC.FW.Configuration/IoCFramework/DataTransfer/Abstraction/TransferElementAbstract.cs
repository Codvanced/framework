using IOC.FW.Shared.Enumerators;
using System;
using System.Configuration;

namespace IOC.FW.Configuration.IoCFramework.DataTransfer.Abstraction
{
    public abstract class TransferElementAbstract
        : ConfigurationElement
    {
        [ConfigurationProperty(ConfigurationVariables.TypeKey, DefaultValue = TransferEnumerator.StructureType.LocalEnvironment, IsKey = true)]
        public virtual TransferEnumerator.StructureType Type
        {
            get
            {
                var ret = default(TransferEnumerator.StructureType);

                Enum.TryParse(
                    this[ConfigurationVariables.TypeKey].ToString(),
                    out ret
                );

                return ret;
            }
        }

        [ConfigurationProperty(ConfigurationVariables.HostKey, DefaultValue = default(string))]
        public virtual string Host
        {
            get
            {
                return (string)this[ConfigurationVariables.HostKey];
            }
        }

        [ConfigurationProperty(ConfigurationVariables.PortKey, DefaultValue = 21)]
        public virtual int Port
        {
            get
            {
                return (int)this[ConfigurationVariables.PortKey];
            }
        }

        [ConfigurationProperty(ConfigurationVariables.UserNameKey, DefaultValue = default(string))]
        public virtual string UserName
        {
            get
            {
                return (string)this[ConfigurationVariables.UserNameKey];
            }
        }

        [ConfigurationProperty(ConfigurationVariables.PasswordKey, DefaultValue = default(string))]
        public virtual string Password
        {
            get
            {
                return (string)this[ConfigurationVariables.PasswordKey];
            }
        }

        [ConfigurationProperty(ConfigurationVariables.PathKey, DefaultValue = default(string))]
        public virtual string Path
        {
            get
            {
                return (string)this[ConfigurationVariables.PathKey];
            }
        }
    }
}