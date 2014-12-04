using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Entity;

namespace IOC.FW.Core.Abstraction.Business
{
    /// <summary>
    /// Abstração para a implementação dos métodos padrões do framework.
    /// </summary>
    /// <typeparam name="TModel">Model (espelho da entidade da base de dados) para tipagem da abstração.</typeparam>
    public interface IBaseBusiness<TModel>
        where TModel : class, new()
    {
        /// <summary>
        /// Implementação de método de IBaseDAO destinado a retornar uma implementação de model
        /// </summary>
        /// <returns>Uma nova instância de TModel</returns>
        TModel Model();

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a retornar uma implementação de lista de model
        /// </summary>
        /// <returns>Uma nova instância de lista de TModel</returns>
        List<TModel> List();

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
        IList<TModel> Select(Func<TModel, bool> where, params Expression<Func<TModel, object>>[] navigationProperties);

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar um unico registro de uma tabela vinculada a uma model. 
        /// </summary>
        /// <param name="where">Delegate contendo parâmetros para composição de WHERE</param>
        /// <param name="navigationProperties">Objetos de uma Model referentes a chaves estrangeiras no database</param>
        /// <returns>Objeto de classe modelo preenchido com registro encontrado</returns>
        TModel SelectSingle(Func<TModel, bool> where, params Expression<Func<TModel, object>>[] navigationProperties);

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
    }
}