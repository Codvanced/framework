using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace IOC.FW.Core.Implementation.Repository
{
    /// <summary>
    /// Classe com o propósito de ter métodos úteis a mapeamento de objetos relacionais
    /// </summary>
    public class ORM
    {
        /// <summary>
        /// Método auxiliar destinado a encontrar as propriedades 
        /// de uma model que estejam decoradas com column attribute. 
        /// </summary>
        /// <param name="tpObject">Tipo de uma class a qual será usada para encontrar as propriedades</param>
        /// <returns>Propriedades encontradas</returns>
        public FormatProperty[] GetProperties(Type tpObject)
        {
            FormatProperty[] formatProperties = null;

            if (tpObject != null)
            {
                var properties = tpObject.GetProperties();

                if (properties != null
                    && properties.Length > 0)
                {
                    formatProperties = new FormatProperty[properties.Length];
                    for (int i = 0; i < formatProperties.Length; i++)
                    {
                        IList<ColumnAttribute> attribute = properties[i]
                            .GetCustomAttributes(typeof(ColumnAttribute), false)
                            .Cast<ColumnAttribute>()
                            .ToList();

                        formatProperties[i] = new FormatProperty
                        {
                            ColumnName =
                                attribute != null
                                && attribute.Count() > 0
                                ? attribute[0].Name
                                : properties[i].Name,
                            PropertyName = properties[i].Name
                        };
                    }
                }
            }

            return formatProperties;
        }

        /// <summary>
        /// Método auxiliar destinado a fazer o de-para de um DataReader para uma model.
        /// </summary>
        /// <typeparam name="TNewModel">Tipo de Model</typeparam>
        /// <param name="reader">DataReader aberto com resultados</param>
        /// <param name="properties">Array de propriedades para utilizar no de para</param>
        /// <returns>List de Model com os valores do DataReader</returns>
        public List<TNewModel> GetModel<TNewModel>(IDataReader reader, FormatProperty[] properties)
            where TNewModel : class
        {
            var list = new List<TNewModel>();

            if (reader != null && !reader.IsClosed)
            {
                TNewModel domain = Activator.CreateInstance<TNewModel>();
                Type domainType = domain.GetType();

                int countFields = reader.FieldCount;
                List<string> columnNames = new List<string>();

                for (int i = 0; i < countFields; i++)
                {
                    columnNames.Add(reader.GetName(i));
                }

                while (reader.Read())
                {
                    domain = Activator.CreateInstance<TNewModel>();

                    foreach (var prop in properties)
                        if (columnNames.Contains(prop.ColumnName) && reader[prop.ColumnName] != DBNull.Value)
                            domainType.GetProperty(prop.PropertyName).SetValue(domain, reader[prop.ColumnName], null);

                    list.Add(domain);
                }
            }

            if (!reader.IsClosed)
                reader.Close();
            
            return list;
        }
    }

    /// <summary>
    /// Classe com o propósito de armazenar as informações úteis de uma Model mapeada.
    /// </summary>
    public class FormatProperty
    {
        /// <summary>
        /// Propriedade para indicar o nome de coluna vinculada
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Propriedade para indicar o nome da propriedade
        /// </summary>
        public string PropertyName { get; set; }
    }
}
