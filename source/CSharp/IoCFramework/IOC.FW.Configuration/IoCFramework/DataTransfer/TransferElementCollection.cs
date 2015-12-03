using System;
using System.Collections.Generic;
using System.Configuration;

namespace IOC.FW.Configuration.IoCFramework.DataTransfer
{
    public class TransferElementCollection
    : ConfigurationElementCollection, IEnumerable<TransferElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TransferElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TransferElement)element).InvariantName;
        }

        protected override string ElementName
        {
            get
            {
                return ConfigurationVariables.TransferKey;
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

        public TransferElement AddTransfer(
            string inavariantName,
            bool enable,
            FromElement from,
            ToElement to
        )
        {
            var element = (TransferElement)CreateNewElement();
            element.InvariantName = inavariantName;
            element.Enable = enable;
            element.From = from;
            element.To = to;
            base.BaseAdd(element);
            return element;
        }

        public new IEnumerator<TransferElement> GetEnumerator()
        {
            var keys = this.BaseGetAllKeys();
            foreach (var key in keys)
            {
                var element = (TransferElement)BaseGet(key);
                if (element.Enable)
                {
                    yield return element;
                }
            }
        }
    }
}