using System;
using System.Collections.Generic;
using System.Configuration;

namespace IOC.FW.Configuration.IoCFramework.ContainerManager
{
    public class ModulesElementCollection
         : ConfigurationElementCollection, IEnumerable<ModuleElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ModuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModuleElement)element).InvariantName;
        }

        protected override string ElementName
        {
            get
            {
                return ConfigurationVariables.ModuleKey;
            }
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            var key = GetElementKey(element);
            if (BaseGet(key) != null)
            {
                throw new InvalidOperationException(key.ToString());
            }

            base.BaseAdd(element);
        }

        protected override void BaseAdd(int index, ConfigurationElement element)
        {
            var key = GetElementKey(element);
            if (BaseGet(key) != null)
            {
                throw new InvalidOperationException(key.ToString());
            }

            base.BaseAdd(index, element);
        }

        public virtual ModuleElement AddModule(
            string inavariantName,
            string assemblyName,
            string className
        )
        {
            var element = (ModuleElement)CreateNewElement();
            element.InvariantName = inavariantName;
            element.AssemblyName = assemblyName;
            element.ClassName = className;
            base.BaseAdd(element);

            return element;
        }

        public new IEnumerator<ModuleElement> GetEnumerator()
        {
            var keys = BaseGetAllKeys();
            foreach (var key in keys)
            {
                var element = (ModuleElement)BaseGet(key);
                yield return element;
            }
        }
    }
}