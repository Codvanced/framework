using System.Configuration;

namespace IOC.FW.Configuration.Thumb
{
    /// <summary>
    /// Classe para representar o elemento da coleção thumb.
    /// </summary>
    public class ThumbElement
        : ConfigurationElement
    {
        /// <summary>
        /// Propriedade para representar o atributo notFound(imagem não encontrada) do elemento thumb.
        /// </summary>
        [ConfigurationProperty("notFoundPath", IsRequired = false)]
        public string NotFoundPath
        {
            get
            {
                return this["notFoundPath"].ToString();
            }
        }

        /// <summary>
        /// Propriedade para representar o atributo width padrão do elemento thumb.
        /// </summary>
        [ConfigurationProperty("defaultWidth", IsRequired = false)]
        public int DefaultWidth
        {
            get
            {
                return (int)this["defaultWidth"];
            }
        }

        /// <summary>
        /// Propriedade para representar o atributo height padrão do elemento thumb.
        /// </summary>
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
