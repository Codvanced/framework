using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace IOC.FW.Configuration.IoCFramework.Thumb
{
    public class ThumbElement
        : ConfigurationElement
    {
        private const string NotFoundPathKey = "notFoundPath";
        private const string DefaultWidthKey = "defaultWidth";
        private const string DefaultHeightKey = "defaultHeight";

        [ConfigurationProperty(NotFoundPathKey)]
        public string NotFoundPath
        {
            get
            {
                return (string)this[NotFoundPathKey];
            }
        }

        [ConfigurationProperty(DefaultWidthKey)]
        public int DefaultWidth
        {
            get
            {
                return (int)this[DefaultWidthKey];
            }
        }

        [ConfigurationProperty(DefaultHeightKey)]
        public int DefaultHeight
        {
            get
            {
                return (int)this[DefaultHeightKey];
            }
        }
    }
}