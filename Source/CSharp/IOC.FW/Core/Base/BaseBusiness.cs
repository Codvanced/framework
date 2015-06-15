using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.FW.Core.Factory;
using IOC.FW.Core.Abstraction.Business;
using IOC.FW.Core.Abstraction.DAO;
using System.Linq.Expressions;
using IOC.FW.Core.Abstraction.Miscellaneous;

namespace IOC.FW.Core.Base
{
    /// <summary>
    /// Classe base para a utilização de Business padronizadas, utilizando Entity Framework como Reposiorio...
    /// </summary>
    /// <typeparam name="TModel">Tipo que representa a classe modelo referente a uma tabela do database</typeparam>
    public class BaseBusiness<TModel>
        : IBaseBusiness<TModel>
        where TModel : class, new()
    {
        /// <summary>
        /// Objeto de IBaseDao, usado para acessar os metodos de DAO
        /// </summary>
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
        /// Implementação de método de IBaseDAO destinado a encontrar todos os registros de uma tabela vinculada a uma Model.
        /// Há possibilidade de incluir objetos referenciais a chaves estrangeiras
        /// </summary>
        /// <param name="order">Delegate contendo parâmetros de ordenação</param>
        /// <param name="navigationProperties">Objetos de uma Model referentes a chaves estrangeiras no database</param>
        /// <returns>Implementação de IList com os registros encontrados.</returns>
        public IList<TModel> SelectAll(
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> order, 
            params Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return this._dao.SelectAll(order, navigationProperties);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar todos os registros de uma tabela vinculada a uma model. 
        /// </summary>
        /// <param name="where">Delegate contendo parâmetros para composição de WHERE</param>
        /// <param name="order">Delegate contendo parâmetros de ordenação</param>
        /// <param name="navigationProperties">Objetos de uma Model referentes a chaves estrangeiras no database</param>
        /// <returns>Implementação de IList com os registros encontrados.</returns>
        public IList<TModel> Select(
            Expression<Func<TModel, bool>> where, 
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> order, 
            params Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return this._dao.Select(where, order, navigationProperties);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar todos os registros de uma tabela vinculada a uma model. 
        /// </summary>
        /// <param name="where">Delegate contendo parâmetros para composição de WHERE</param>
        /// <param name="navigationProperties">Objetos de uma Model referentes a chaves estrangeiras no database</param>
        /// <returns>Implementação de IList com os registros encontrados.</returns>
        public IList<TModel> Select(
            Expression<Func<TModel, bool>> where,
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
            Expression<Func<TModel, bool>> where,
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
        /// Implementação de método de IBaseDAO destinado a atualizar uma coleção de registros.
        /// </summary>
        /// <param name="items">Coleção de registros a atualizar na base</param>
        public void Update(params TModel[] items)
        {
            this._dao.Update(items);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a atualizar propriedades específicas de um objeto.
        /// </summary>
        /// <param name="item">Item a atualizar na base</param>
        /// <param name="properties">Propriedades do objeto a atualizar na base</param>
        public void Update(TModel item, Expression<Func<TModel, object>>[] properties)
        {
            this._dao.Update(item, properties);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a excluir (logicamente ou fisicamente) uma coleção de registros.
        /// </summary>
        /// <param name="items">Coleção de registros a deletar da base</param>
        public void Delete(params TModel[] items)
        {
            this._dao.Delete(items);
        }

        /// <summary>
        /// Implementacao de método para atualizar a prioridade do elemento na tabela
        /// </summary>
        /// <typeparam name="TPriorityModel">Tipo do model que implementa IPrioritySortable</typeparam>
        /// <param name="items">Lista de models que implementam IPrioritySortable</param>
        public void UpdatePriority<TPriorityModel>(params TPriorityModel[] items) where TPriorityModel : TModel, IPrioritySortable
        {
            this._dao.UpdatePriority<TPriorityModel>(items);
        }

        /// <summary>
        /// Implementação de método para retornar um count da tabela vinculada ao objeto
        /// </summary>
        /// <returns>Quantidade de registros</returns>
        public int Count()
        {
            return this.Count(m => true);
        }

        /// <summary>
        /// Implementação de método para retornar um count da tabela vinculada ao objeto
        /// </summary>
        /// <param name="where">Filtro de busca</param>
        /// <returns>Quantidade de registros</returns>
        public int Count(Expression<Func<TModel, bool>> where)
        {
            return this._dao.Count(where);
        }

        /// <summary>
        /// Implementação de método para retornar um count da tabela vinculada ao objeto
        /// </summary>
        /// <returns>Quantidade de registros</returns>
        public long LongCount()
        {
            return this.LongCount(m => true);
        }

        /// <summary>
        /// Implementação de método para retornar um count da tabela vinculada ao objeto
        /// </summary>
        /// <param name="where">Filtro de busca</param>
        /// <returns>Quantidade de registros</returns>
        public long LongCount(Expression<Func<TModel, bool>> where)
        {
            return this._dao.LongCount(where);
        }
    }
}