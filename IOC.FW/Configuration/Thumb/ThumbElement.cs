using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace IOC.FW.Configuration.Thumb
{
    /// <summary>
    /// Classe representa o elemento da coleção thumb.
    /// </summary>
    public class ThumbElement
        : ConfigurationElement
    {
        /// <summary>
        /// Propriedade representa o atributo notFound(imagem não encontrada) do elemento thumb.
        /// </summary>
        [ConfigurationProperty("notFoundPath", IsRequired = false)]
        public string NotFoundPath
        {
            get
            {
                return this["notFoundPath"].ToString();
            }
        }

        [ConfigurationProperty("defaultWidth", IsRequired = false)]
        public int DefaultWidth
        {
            get
            {
                return (int)this["defaultWidth"];
            }
        }

        [ConfigurationProperty("defaultHeight", IsRequired = false)]
        public int DefaultHeight
        {
            get
            {
                return (int)this["defaultHeight"];
            }
        }
    }
}
