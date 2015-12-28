using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web.Caching;

namespace IOC.FW.Configuration.IoCFramework.Thumb
{
    public class ThumbElement
        : ConfigurationElement
    {
        [ConfigurationProperty(ConfigurationVariables.NotFoundPathKey)]
        public virtual string NotFoundPath
        {
            get
            {
                return (string)this[ConfigurationVariables.NotFoundPathKey];
            }
        }

        [ConfigurationProperty(ConfigurationVariables.DefaultWidthKey)]
        public virtual int DefaultWidth
        {
            get
            {
                return (int)this[ConfigurationVariables.DefaultWidthKey];
            }
        }

        [ConfigurationProperty(ConfigurationVariables.DefaultHeightKey)]
        public virtual int DefaultHeight
        {
            get
            {
                return (int)this[ConfigurationVariables.DefaultHeightKey];
            }
        }

        [ConfigurationProperty(ConfigurationVariables.EnableCacheKey)]
        public virtual bool EnableCache
        {
            get
            {
                return (bool)this[ConfigurationVariables.EnableCacheKey];
            }
        }

        [ConfigurationProperty(ConfigurationVariables.ExpirationKey)]
        public virtual double Expiration
        {
            get
            {
                return (double)this[ConfigurationVariables.ExpirationKey];
            }
        }

        [ConfigurationProperty(ConfigurationVariables.SlidingExpirationKey)]
        public virtual double SlidingExpiration
        {
            get
            {
                return (double)this[ConfigurationVariables.SlidingExpirationKey];
            }
        }

        [ConfigurationProperty(ConfigurationVariables.CachePriorityKey)]
        public virtual CacheItemPriority CachePriority
        {
            get
            {
                return (CacheItemPriority)this[ConfigurationVariables.CachePriorityKey];
            }
        }
    }
}