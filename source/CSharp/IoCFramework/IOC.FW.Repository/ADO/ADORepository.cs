using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using IOC.FW.Core.Abstraction.Repository;
using IOC.FW.Core.Abstraction.Miscellaneous;
using IOC.FW.Shared.Enumerators;

namespace IOC.FW.Repository.ADO
{
    public class ADORepository<TModel>
        : IRepository<TModel>
        where TModel : class, new()
    {

        public RepositoryEnumerator.RepositoryType Type
        {
            get
            {
                return RepositoryEnumerator.RepositoryType.ADO;
            }
        }

        public IList<TModel> ExecuteQuery(string sql, Dictionary<string, object> parameters, CommandType cmdType = CommandType.Text)
        {
            throw new NotImplementedException();
        }

        public IList<TModel> ExecuteQuery(string sql, List<Tuple<ParameterDirection, string, object>> parametersWithDirection, CommandType cmdType)
        {
            throw new NotImplementedException();
        }

        public object ExecuteScalar(string sql, Dictionary<string, object> parameters = null, System.Data.CommandType cmdType = CommandType.Text)
        {
            throw new NotImplementedException();
        }

        public TModel Model()
        {
            throw new NotImplementedException();
        }

        public List<TModel> List()
        {
            throw new NotImplementedException();
        }

        public IList<TModel> SelectAll(params System.Linq.Expressions.Expression<Func<TModel, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public IList<TModel> SelectAll(Func<IQueryable<TModel>, IOrderedQueryable<TModel>> order, params System.Linq.Expressions.Expression<Func<TModel, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public IList<TModel> Select(System.Linq.Expressions.Expression<Func<TModel, bool>> where, Func<IQueryable<TModel>, IOrderedQueryable<TModel>> order, params System.Linq.Expressions.Expression<Func<TModel, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public IList<TModel> Select(System.Linq.Expressions.Expression<Func<TModel, bool>> where, params System.Linq.Expressions.Expression<Func<TModel, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public TModel SelectSingle(System.Linq.Expressions.Expression<Func<TModel, bool>> where, params System.Linq.Expressions.Expression<Func<TModel, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public void Insert(params TModel[] items)
        {
            throw new NotImplementedException();
        }

        public void Update(params TModel[] items)
        {
            throw new NotImplementedException();
        }

        public void Update(TModel item, System.Linq.Expressions.Expression<Func<TModel, object>>[] properties)
        {
            throw new NotImplementedException();
        }

        public void Delete(params TModel[] items)
        {
            throw new NotImplementedException();
        }

        public void UpdatePriority<TPriorityModel>(params TPriorityModel[] items) where TPriorityModel : TModel, IPrioritySortable
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public int Count(System.Linq.Expressions.Expression<Func<TModel, bool>> where)
        {
            throw new NotImplementedException();
        }

        public long LongCount()
        {
            throw new NotImplementedException();
        }

        public long LongCount(System.Linq.Expressions.Expression<Func<TModel, bool>> where)
        {
            throw new NotImplementedException();
        }

        public void SetConnection(string nameOrConnectionString)
        {
            throw new NotImplementedException();
        }

        public void SetConnection(System.Data.Common.DbConnection connection, System.Data.Common.DbTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public void ExecuteWithTransaction(IsolationLevel isolation, IBaseTransaction[] DAOs, Action<System.Data.Common.DbTransaction> transactionExecution)
        {
            throw new NotImplementedException();
        }
    }
}