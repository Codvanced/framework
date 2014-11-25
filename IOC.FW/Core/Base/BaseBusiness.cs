using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.FW.Core.Factory;
using IOC.FW.Core.Abstraction.Business;
using IOC.FW.Core.Abstraction.DAO;
using System.Linq.Expressions;

namespace IOC.FW.Core.Base
{
    /// <summary>
    /// Classe base para a utilização de Business padronizadas, utilizando Entity Framework como Reposiorio Base...
    /// </summary>
    /// <typeparam name="TModel">Tipo que representa a classe modelo referente a uma tabela do database</typeparam>
    public class BaseBusiness<TModel>
        : IBaseBusiness<TModel>
        where TModel : class, new()
    {
        private readonly IBaseDAO<TModel> _dao;

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a retornar uma implementação de model
        /// </summary>
        /// <returns>Uma nova instância de TModel</returns>
        public TModel Model() 
        {
            return new TModel();
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a retornar uma implementação de lista de model
        /// </summary>
        /// <returns>Uma nova instância de lista de TModel</returns>
        public List<TModel> List() 
        {
            return new List<TModel>();
        }

        /// <summary>
        /// Constructor recebendo uma implementação de DAO
        /// </summary>
        /// <param name="dao">Implementação de Base DAO</param>
        public BaseBusiness(IBaseDAO<TModel> dao)
        {
            this._dao = dao;
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar todos os registros de uma tabela vinculada a uma Model.
        /// Há possibilidade de incluir objetos referenciais a chaves estrangeiras
        /// </summary>
        /// <param name="navigationProperties">Objetos de uma Model referentes a chaves estrangeiras no database</param>
        /// <returns>Implementação de IList com os registros encontrados.</returns>
        public IList<TModel> SelectAll(
            params Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return this._dao.SelectAll(navigationProperties);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar todos os registros de uma tabela vinculada a uma model. 
        /// </summary>
        /// <param name="where">Delegate contendo parâmetros para composição de WHERE</param>
        /// <param name="navigationProperties">Objetos de uma Model referentes a chaves estrangeiras no database</param>
        /// <returns>Implementação de IList com os registros encontrados.</returns>
        public IList<TModel> Select(
            Func<TModel, bool> where,
            params Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return this._dao.Select(where, navigationProperties);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar um unico registro de uma tabela vinculada a uma model. 
        /// </summary>
        /// <param name="where">Delegate contendo parâmetros para composição de WHERE</param>
        /// <param name="navigationProperties">Objetos de uma Model referentes a chaves estrangeiras no database</param>
        /// <returns>Objeto de classe modelo preenchido com registro encontrado</returns>
        public TModel SelectSingle(
            Func<TModel, bool> where,
            params Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return this._dao.SelectSingle(where, navigationProperties);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a atualizar uma coleção de registros.
        /// </summary>
        /// <param name="items">Coleção de registros a inserir na base</param>
        public void Insert(params TModel[] items)
        {
            this._dao.Insert(items);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a atualizar uma coleção muitos de registros. Em uma única transaction.
        /// </summary>
        /// <param name="items">Coleção de registros a inserir na base</param>
        public void BulkInsert(params TModel[] items)
        {
            this._dao.BulkInsert(items);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a atualizar uma coleção de registros.
        /// </summary>
        /// <param name="items">Coleção de registros a atualizar na base</param>
        public void Update(params TModel[] items)
        {
            this._dao.Update(items);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a atualizar uma coleção de muitos registros. Em uma única transaction.
        /// </summary>
        /// <param name="items">Coleção de registros a atualizar na base</param>
        public void BulkUpdate(params TModel[] items)
        {
            this._dao.BulkUpdate(items);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a excluir (logicamente ou fisicamente) uma coleção de registros.
        /// </summary>
        /// <param name="items">Coleção de registros a deletar da base</param>
        public void Delete(params TModel[] items)
        {
            this._dao.Delete(items);
        }

    }
}