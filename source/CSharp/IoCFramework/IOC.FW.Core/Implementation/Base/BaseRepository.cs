using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using IOC.FW.Core.Abstraction.Repository;
using IOC.FW.Core.Abstraction.Miscellaneous;
using IOC.FW.Shared.Enumerators;

namespace IOC.FW.Core.Implementation.Base
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
            List<Tuple<ParameterDirection, string, object>> parametersWithDirection,
            CommandType cmdType
        )
        {
            return _dao.ExecuteQuery(sql, parametersWithDirection, cmdType);
        }

        public TModel Model()
        {
            return new TModel();
        }

        public List<TModel> List()
        {
            return new List<TModel>();
        }

        public IList<TModel> SelectAll(
            params Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return _dao.SelectAll(navigationProperties);
        }

        public IList<TModel> SelectAll(
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> order,
            params Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return _dao.SelectAll(order, navigationProperties);
        }

        public IList<TModel> Select(
            Expression<Func<TModel, bool>> where,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> order,
            params Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return _dao.Select(where, order, navigationProperties);
        }

        public IList<TModel> Select(
            Expression<Func<TModel, bool>> where,
            params Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return _dao.Select(where, navigationProperties);
        }

        public TModel SelectSingle(
            Expression<Func<TModel, bool>> where,
            params Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return _dao.SelectSingle(where, navigationProperties);
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