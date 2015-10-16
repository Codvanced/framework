using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using IOC.FW.Core.Abstraction.Repository;
using IOC.FW.Core.Abstraction.Miscellaneous;
using IOC.FW.Core.Implementation.Repository;

namespace IOC.FW.Core.Implementation.Base
{
    public class BaseRepository<TModel>
        : IRepository<TModel>
        where TModel : class, new()
    {
        private readonly IRepository<TModel> _dao;
        public Enumerators.RepositoryType Type
        {
            get
            {
                return this._dao.Type;
            }
        }

        public BaseRepository(IRepository<TModel> dao)
        {
            this._dao = dao;
        }
        
        public IList<TModel> ExecuteQuery(
            string sql,
            Dictionary<string, object> parameters,
            CommandType cmdType = CommandType.Text
        )
        {
            return this._dao.ExecuteQuery(sql, parameters, cmdType);
        }

        public object ExecuteScalar(
            string sql,
            Dictionary<string, object> parameters = null,
            CommandType cmdType = CommandType.Text
        )
        {
            return this._dao.ExecuteScalar(sql, parameters, cmdType);
        }

        public IList<TModel> ExecuteQuery(
            string sql,
            List<Tuple<ParameterDirection, string, object>> parametersWithDirection,
            CommandType cmdType
        )
        {
            return this._dao.ExecuteQuery(sql, parametersWithDirection, cmdType);
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
            return this._dao.SelectAll(navigationProperties);
        }

        public IList<TModel> SelectAll(
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> order,
            params Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return this._dao.SelectAll(order, navigationProperties);
        }

        public IList<TModel> Select(
            Expression<Func<TModel, bool>> where,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> order,
            params Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return this._dao.Select(where, order, navigationProperties);
        }

        public IList<TModel> Select(
            Expression<Func<TModel, bool>> where,
            params Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return this._dao.Select(where, navigationProperties);
        }

        public TModel SelectSingle(
            Expression<Func<TModel, bool>> where,
            params Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            return this._dao.SelectSingle(where, navigationProperties);
        }

        public void Insert(params TModel[] items)
        {
            this._dao.Insert(items);
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
            this._dao.Delete(items);
        }

        public void UpdatePriority<TPriorityModel>(
            params TPriorityModel[] items
        ) where TPriorityModel : TModel, IPrioritySortable
        {
            this._dao.UpdatePriority<TPriorityModel>(items);
        }

        public int Count()
        {
            return this._dao.Count();
        }

        public int Count(
            Expression<Func<TModel, bool>> where
        )
        {
            return this._dao.Count(where);
        }

        public long LongCount()
        {
            return this._dao.LongCount();
        }

        public long LongCount(
            Expression<Func<TModel, bool>> where
        )
        {
            return this._dao.LongCount(where);
        }

        public void SetConnection(
            string nameOrConnectionString
        )
        {
            this._dao.SetConnection(nameOrConnectionString);
        }

        public void SetConnection(
            DbConnection connection, 
            DbTransaction transaction
        )
        {
            this._dao.SetConnection(connection, transaction);
        }

        public void ExecuteWithTransaction(
            IsolationLevel isolation,
            IBaseTransaction[] DAOs,
            Action<DbTransaction> transactionExecution
        )
        {
            this._dao.ExecuteWithTransaction(isolation, DAOs, transactionExecution);
        }
    }
}