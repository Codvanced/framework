using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using IOC.FW.Abstraction.Repository;
using IOC.FW.Abstraction.Miscellaneous;
using IOC.FW.Shared.Enumerators;
using IOC.FW.Shared.Model.Repository;

namespace IOC.FW.Repository
{
    public class BaseRepository<TModel>
        : IRepository<TModel>
        where TModel : class, new()
    {
        private readonly IRepository<TModel> _dao;
        public RepositoryEnumerator.RepositoryType Type
        {
            get
            {
                return _dao.Type;
            }
        }

        public BaseRepository(IRepository<TModel> dao)
        {
            _dao = dao;
        }

        public IList<TModel> ExecuteQuery(
            string sql,
            Dictionary<string, object> parameters,
            CommandType cmdType = CommandType.Text
        )
        {
            return _dao.ExecuteQuery(sql, parameters, cmdType);
        }

        public object ExecuteScalar(
            string sql,
            Dictionary<string, object> parameters = null,
            CommandType cmdType = CommandType.Text
        )
        {
            return _dao.ExecuteScalar(sql, parameters, cmdType);
        }

        public IList<TModel> ExecuteQuery(
            string sql,
            List<ParameterData> parametersWithDirection,
            CommandType cmdType
        )
        {
            return _dao.ExecuteQuery(sql, parametersWithDirection, cmdType);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar todos os registros de uma tabela vinculada a uma Model.
        /// </summary>
        /// <returns>Implementação de IList com os registros encontrados.</returns>
        public IList<TModel> Select()
        {
            return _dao.Select();
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar todos os registros de uma tabela vinculada a uma Model.
        /// </summary>
        /// <param name="navigationProperties">Objetos de uma Model referentes a chaves estrangeiras no database</param>
        /// <returns>Implementação de IList com os registros encontrados.</returns>
        public IList<TModel> Select(
            params Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return _dao.Select(navigationProperties);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar todos os registros de uma tabela vinculada a uma Model.
        /// Há possibilidade de incluir objetos referenciais a chaves estrangeiras
        /// </summary>
        /// <param name="order">Delegate contendo parâmetros de ordenação</param>
        /// <returns>Implementação de IList com os registros encontrados.</returns>
        public IList<TModel> Select(
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> order
        )
        {
            return _dao.Select(order);
        }


        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar todos os registros de uma tabela vinculada a uma Model.
        /// Há possibilidade de incluir objetos referenciais a chaves estrangeiras
        /// </summary>
        /// <param name="order">Delegate contendo parâmetros de ordenação</param>
        /// <param name="navigationProperties">Objetos de uma Model referentes a chaves estrangeiras no database</param>
        /// <returns>Implementação de IList com os registros encontrados.</returns>
        public IList<TModel> Select(
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> order,
            Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return _dao.Select(order, navigationProperties);
        }


        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar todos os registros de uma tabela vinculada a uma model. 
        /// </summary>
        /// <param name="where">Delegate contendo parâmetros para composição de WHERE</param>
        /// <returns>Implementação de IList com os registros encontrados.</returns>
        public IList<TModel> Select(Expression<Func<TModel, bool>> where)
        {
            return _dao.Select(where);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar todos os registros de uma tabela vinculada a uma model. 
        /// </summary>
        /// <param name="where">Delegate contendo parâmetros para composição de WHERE</param>
        /// <param name="navigationProperties">Objetos de uma Model referentes a chaves estrangeiras no database</param>
        /// <returns>Implementação de IList com os registros encontrados.</returns>
        public IList<TModel> Select(
            Expression<Func<TModel, bool>> where,
            Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return _dao.Select(where, navigationProperties);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar todos os registros de uma tabela vinculada a uma model. 
        /// </summary>
        /// <param name="where">Delegate contendo parâmetros para composição de WHERE</param>
        /// <param name="order">Delegate contendo parâmetros de ordenação</param>
        /// <returns>Implementação de IList com os registros encontrados.</returns>
        public IList<TModel> Select(
            Expression<Func<TModel, bool>> where,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> order
        )
        {
            return _dao.Select(where, order);
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
            Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return _dao.Select(where, order, navigationProperties);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar um unico registro de uma tabela vinculada a uma model. 
        /// </summary>
        /// <param name="order">Delegate contendo parâmetros de ordenação</param>
        /// <returns>Objeto de classe modelo preenchido com registro encontrado</returns>
        public TModel SelectSingle(
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> order
        )
        {
            return _dao.SelectSingle(order);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar um unico registro de uma tabela vinculada a uma model. 
        /// </summary>
        /// <param name="where">Delegate contendo parâmetros para composição de WHERE</param>
        /// <returns>Objeto de classe modelo preenchido com registro encontrado</returns>
        public TModel SelectSingle(
            Expression<Func<TModel, bool>> where
        )
        {
            return _dao.SelectSingle(where);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar um unico registro de uma tabela vinculada a uma model. 
        /// <param name="where">Delegate contendo parâmetros para composição de WHERE</param>
        /// <param name="order">Delegate contendo parâmetros de ordenação</param>
        /// </summary>
        /// <returns>Objeto de classe modelo preenchido com registro encontrado</returns>
        public TModel SelectSingle(
            Expression<Func<TModel, bool>> where,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> order
        )
        {
            return _dao.SelectSingle(where, order);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar um unico registro de uma tabela vinculada a uma model. 
        /// </summary>
        /// <param name="where">Delegate contendo parâmetros para composição de WHERE</param>
        /// <param name="navigationProperties">Objetos de uma Model referentes a chaves estrangeiras no database</param>
        /// <returns>Objeto de classe modelo preenchido com registro encontrado</returns>
        public TModel SelectSingle(
            Expression<Func<TModel, bool>> where,
            Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return _dao.SelectSingle(where, navigationProperties);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar um unico registro de uma tabela vinculada a uma model. 
        /// </summary>
        /// <param name="order">Delegate contendo parâmetros de ordenação</param>
        /// <param name="navigationProperties">Objetos de uma Model referentes a chaves estrangeiras no database</param>
        /// <returns>Objeto de classe modelo preenchido com registro encontrado</returns>
        public TModel SelectSingle(
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> order,
            Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return _dao.SelectSingle(order, navigationProperties);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar um unico registro de uma tabela vinculada a uma model. 
        /// </summary>
        /// <param name="where">Delegate contendo parâmetros para composição de WHERE</param>
        /// <param name="order">Delegate contendo parâmetros de ordenação</param>
        /// <param name="navigationProperties">Objetos de uma Model referentes a chaves estrangeiras no database</param>
        /// <returns>Objeto de classe modelo preenchido com registro encontrado</returns>
        public TModel SelectSingle(
            Expression<Func<TModel, bool>> where,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> order,
            Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return _dao.SelectSingle(where, order, navigationProperties);
        }

        public TResult Max<TResult>(
            Expression<Func<TModel, bool>> where,
            Expression<Func<TModel, TResult>> maxSelector
        )
        {
            return _dao.Max(where, maxSelector);
        }

        public TResult Min<TResult>(
            Expression<Func<TModel, bool>> where,
            Expression<Func<TModel, TResult>> maxSelector
        )
        {
            return _dao.Min(where, maxSelector);
        }


        public void Insert(params TModel[] items)
        {
            _dao.Insert(items);
        }

        public void Update(params TModel[] items)
        {
            _dao.Update(items);
        }

        public void Update(
            TModel item,
            Expression<Func<TModel, object>>[] properties
        )
        {
            _dao.Update(item, properties);
        }

        public void Delete(params TModel[] items)
        {
            _dao.Delete(items);
        }

        public void UpdatePriority<TPriorityModel>(
            params TPriorityModel[] items
        ) where TPriorityModel : TModel, IPrioritySortable
        {
            _dao.UpdatePriority<TPriorityModel>(items);
        }

        public int Count()
        {
            return _dao.Count();
        }

        public int Count(
            Expression<Func<TModel, bool>> where
        )
        {
            return _dao.Count(where);
        }

        public long LongCount()
        {
            return _dao.LongCount();
        }

        public long LongCount(
            Expression<Func<TModel, bool>> where
        )
        {
            return _dao.LongCount(where);
        }

        public void SetConnection(
            string nameOrConnectionString
        )
        {
            _dao.SetConnection(nameOrConnectionString);
        }

        public void SetConnection(
            DbConnection connection, 
            DbTransaction transaction
        )
        {
            _dao.SetConnection(connection, transaction);
        }

        public void ExecuteWithTransaction(
            IsolationLevel isolation,
            IBaseTransaction[] DAOs,
            Action<DbTransaction> transactionExecution
        )
        {
            _dao.ExecuteWithTransaction(isolation, DAOs, transactionExecution);
        }
    }
}