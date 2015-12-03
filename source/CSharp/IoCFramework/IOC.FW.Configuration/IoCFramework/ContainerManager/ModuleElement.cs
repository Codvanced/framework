using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace IOC.FW.Configuration.IoCFramework.ContainerManager
{
    public class ModuleElement
        : ConfigurationElement
    {
        [ConfigurationProperty(ConfigurationVariables.InvariantNameKey, IsRequired = true, IsKey = true)]
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

        [ConfigurationProperty(ConfigurationVariables.AssemblyNameKey, IsRequired = true)]
        public virtual string AssemblyName
        {
            get
            {
                return (string)this[ConfigurationVariables.AssemblyNameKey];
            }
            set
            {
                this[ConfigurationVariables.AssemblyNameKey] = value;
            }
        }

        [ConfigurationProperty(ConfigurationVariables.ClassNameKey, IsRequired = true)]
        public virtual string ClassName
        {
            get
            {
                return (string)this[ConfigurationVariables.ClassNameKey];
            }
            set
            {
                this[ConfigurationVariables.ClassNameKey] = value;
            }
        }
    }
}