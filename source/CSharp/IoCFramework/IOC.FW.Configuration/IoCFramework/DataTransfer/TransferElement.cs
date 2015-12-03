using System.Configuration;

namespace IOC.FW.Configuration.IoCFramework.DataTransfer
{
    public class TransferElement
        : ConfigurationElement
    {
        [ConfigurationProperty(ConfigurationVariables.InvariantNameKey, IsRequired = true)]
        public virtual string InvariantName
        {
            get
            {
                return (string)this[ConfigurationVariables.InvariantNameKey];
            }
            set
            {
                this[ConfigurationVariables.InvariantNameKey] = value;
            }
        }

        [ConfigurationProperty(ConfigurationVariables.EnableKey, DefaultValue = true)]
        public virtual bool Enable
        {
            get
            {
                return (bool)this[ConfigurationVariables.EnableKey];
            }
            set
            {
                this[ConfigurationVariables.EnableKey] = value;
            }
        }

        [ConfigurationProperty(ConfigurationVariables.FromEmptyWarningEnableKey, DefaultValue = false)]
        public virtual bool FromEmptyWarningEnable
        {
            get
            {
                return (bool)this[ConfigurationVariables.FromEmptyWarningEnableKey];
            }
            set
            {
                this[ConfigurationVariables.FromEmptyWarningEnableKey] = value;
            }
        }

        [ConfigurationProperty(ConfigurationVariables.FromEmptyWarningToKey)]
        public virtual string FromEmptyWarningTo
        {
            get
            {
                return (string)this[ConfigurationVariables.FromEmptyWarningToKey];
            }
            set
            {
                this[ConfigurationVariables.FromEmptyWarningToKey] = value;
            }
        }

        [ConfigurationProperty(ConfigurationVariables.FromEmptyWarningCcKey)]
        public virtual string FromEmptyWarningCc
        {
            get
            {
                return (string)this[ConfigurationVariables.FromEmptyWarningCcKey];
            }
            set
            {
                this[ConfigurationVariables.FromEmptyWarningCcKey] = value;
            }
        }

        [ConfigurationProperty(ConfigurationVariables.FromEmptyWarningSubjectKey)]
        public virtual string FromEmptyWarningSubject
        {
            get
            {
                return (string)this[ConfigurationVariables.FromEmptyWarningSubjectKey];
            }
            set
            {
                this[ConfigurationVariables.FromEmptyWarningSubjectKey] = value;
            }
        }

        [ConfigurationProperty(ConfigurationVariables.FromEmptyWarningBodyKey)]
        public virtual string FromEmptyWarningBody
        {
            get
            {
                return (string)this[ConfigurationVariables.FromEmptyWarningBodyKey];
            }
            set
            {
                this[ConfigurationVariables.FromEmptyWarningBodyKey] = value;
            }
        }

        [ConfigurationProperty(ConfigurationVariables.FromKey)]
        public virtual FromElement From
        {
            get
            {
                return (FromElement)this[ConfigurationVariables.FromKey];
            }
            set
            {
                this[ConfigurationVariables.FromKey] = value;
            }
        }

        [ConfigurationProperty(ConfigurationVariables.ToKey)]
        public virtual ToElement To
        {
            get
            {
                return (ToElement)this[ConfigurationVariables.ToKey];
            }
            set
            {
                this[ConfigurationVariables.ToKey] = value;
            }
        }
    }
}