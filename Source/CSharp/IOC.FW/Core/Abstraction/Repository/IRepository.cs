using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data;
using IOC.FW.Core.Abstraction.Miscellaneous;
using IOC.FW.Core.Abstraction.Business;
using IOC.FW.Core.Abstraction.Repository.Connection;

namespace IOC.FW.Core.Abstraction.Repository
{
    /// <summary>
    /// Interface responsável por padronizar DAOs de projetos
    /// </summary>
    /// <typeparam name="TModel">Tipo da classe modelo</typeparam>
    public interface IRepository<TModel>
        : IBaseBusiness<TModel>, IConnectionConfigurable, IBaseTransaction
        where TModel: class, new()
    {
        Enumerators.RepositoryType Type { get; }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a executar querys customizadas e procedures.
        /// </summary>
        /// <param name="sql">Query ou nome de procedure</param>
        /// <param name="parameters">Dicionaário com os parâmetros e valores a incluir</param>
        /// <param name="cmdType">Tipo de execução: query/procedure</param>
        /// <returns>Implementação de listagem contendo os resultados obtidos</returns>
        IList<TModel> ExecuteQuery(
            string sql, 
            Dictionary<string, object> parameters, 
            CommandType cmdType = CommandType.Text
        );

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a executar querys customizadas e procedures.
        /// </summary>
        /// <param name="sql">Query ou nome de procedure</param>
        /// <param name="parametersWithDirection">Lista de tupla com a direção dos parâmetros e valores a incluir</param>
        /// <param name="cmdType">Tipo de execução: query/procedure</param>
        /// <returns>Objeto com o resultado obtido</returns>
        IList<TModel> ExecuteQuery(
            string sql,
            List<Tuple<ParameterDirection, string, object>> parametersWithDirection,
            CommandType cmdType
        );

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a executar querys customizadas e procedures.
        /// </summary>
        /// <param name="sql">Query ou nome de procedure</param>
        /// <param name="parameters">Dicionaário com os parâmetros e valores a incluir</param>
        /// <param name="cmdType">Tipo de execução: query/procedure</param>
        /// <returns>Objeto com o resultado obtido</returns>
        object ExecuteScalar(
            string sql,
            Dictionary<string, object> parameters = null,
            CommandType cmdType = CommandType.Text
        );
    }
}