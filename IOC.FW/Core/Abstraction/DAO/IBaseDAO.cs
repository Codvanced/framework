using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data;

namespace IOC.FW.Core.Abstraction.DAO
{
    /// <summary>
    /// Interface responsável por padronizar DAOs de projetos
    /// </summary>
    /// <typeparam name="TModel">Tipo da classe modelo</typeparam>
    public interface IBaseDAO<TModel>
        where TModel: class, new()
    {
        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar todos os registros de uma tabela vinculada a uma Model.
        /// Há possibilidade de incluir objetos referenciais a chaves estrangeiras
        /// </summary>
        /// <param name="navigationProperties">Objetos de uma Model referentes a chaves estrangeiras no database</param>
        /// <returns>Implementação de IList com os registros encontrados.</returns>
        IList<TModel> SelectAll(params Expression<Func<TModel, object>>[] navigationProperties);

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar todos os registros de uma tabela vinculada a uma model. 
        /// </summary>
        /// <param name="where">Delegate contendo parâmetros para composição de WHERE</param>
        /// <param name="navigationProperties">Objetos de uma Model referentes a chaves estrangeiras no database</param>
        /// <returns>Implementação de IList com os registros encontrados.</returns>
        IList<TModel> Select(
            Func<TModel, bool> where, 
            params Expression<Func<TModel, object>>[] navigationProperties
        );

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar um unico registro de uma tabela vinculada a uma model. 
        /// </summary>
        /// <param name="where">Delegate contendo parâmetros para composição de WHERE</param>
        /// <param name="navigationProperties">Objetos de uma Model referentes a chaves estrangeiras no database</param>
        /// <returns>Objeto de classe modelo preenchido com registro encontrado</returns>
        TModel SelectSingle(
            Func<TModel, bool> where, 
            params Expression<Func<TModel, object>>[] navigationProperties
        );

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a atualizar uma coleção de registros.
        /// </summary>
        /// <param name="items">Coleção de registros a inserir na base</param>
        void Insert(params TModel[] items);

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a atualizar uma coleção muitos de registros. Em uma única transaction.
        /// </summary>
        /// <param name="items">Coleção de registros a inserir na base</param>
        void BulkInsert(params TModel[] items);

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a atualizar uma coleção de registros.
        /// </summary>
        /// <param name="items">Coleção de registros a atualizar na base</param>
        void Update(params TModel[] items);

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a atualizar uma coleção de muitos registros. Em uma única transaction.
        /// </summary>
        /// <param name="items">Coleção de registros a atualizar na base</param>
        void BulkUpdate(params TModel[] items);

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a excluir (logicamente ou fisicamente) uma coleção de registros.
        /// </summary>
        /// <param name="items">Coleção de registros a deletar da base</param>
        void Delete(params TModel[] items);

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
        /// <param name="parameters">Dicionaário com os parâmetros e valores a incluir</param>
        /// <param name="cmdType">Tipo de execução: query/procedure</param>
        /// <returns>Objeto com o resultado obtido</returns>
        object ExecuteScalar(
            string sql,
            Dictionary<string, object> parameters = null,
            CommandType cmdType = CommandType.Text
        );

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a expor o Context de maneira segura para a DAO.
        /// </summary>
        /// <param name="lambda">Delegate para expor o Context e retornar uma Model preenchida</param>
        /// <returns>Objeto de classe modelo preenchido com registro encontrado</returns>
        TModel Exec(Func<DbSet<TModel>, TModel> lambda);

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a expor o Context de maneira segura para a DAO.
        /// </summary>
        /// <typeparam name="TGenericModel">Tipo de Model de entrada para preencher DbSet</typeparam>
        /// <param name="lambda">Delegate para expor o Context e retornar uma Model preenchida</param>
        /// <returns>Model de retorno preenchida com registro encontrado</returns>
        TGenericModel Exec<TGenericModel>(Func<DbSet<TGenericModel>, TGenericModel> lambda)
            where TGenericModel : class, new();

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a expor o Context de maneira segura para a DAO.
        /// </summary>
        /// <typeparam name="TGenericModel">Tipo de Model de entrada para preencher DbSet</typeparam>
        /// <typeparam name="TGenericModeOut">Tipo de Model de saida</typeparam>
        /// <param name="lambda">Delegate para expor o Context e retornar uma Model preenchida</param>
        /// <returns>Model de retorno preenchida com registro encontrado</returns>
        TGenericModeOut Exec<TGenericModel, TGenericModeOut>(
            Func<DbSet<TGenericModel>,
            TGenericModeOut> lambda
        )
            where TGenericModel : class, new();

        IList<TModel> ExecuteQuery(
            string sql,
            List<Tuple<ParameterDirection, string, object>> parametersWithDirection,
            CommandType cmdType
        );
    }
}