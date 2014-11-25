using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Configuration;
using IOC.FW.Core.Abstraction.DAO;
using System.Linq.Expressions;
using IOC.FW.Core.Abstraction.Model;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using IOC.FW.Core.Database;
using System.ComponentModel.DataAnnotations;

namespace IOC.FW.Core.Base
{
    /// <summary>
    /// Classe base para a utilização de Repositorios padronizadas, utilizando Entity Framework como Reposiorio Base...
    /// </summary>
    /// <typeparam name="TModel">Tipo que representa a classe modelo referente a uma tabela do database</typeparam>
    public class BaseRepository<TModel>
        : IBaseDAO<TModel>
        where TModel : class, new()
    {
        private string nameOrConnectionString = string.Empty;

        /// <summary>
        /// Construtor padrão, inicializa a string de conexão com o parametrizado em ConnectionStrings.config (Name: DefaultConnection)
        /// </summary>
        public BaseRepository()
        {
            nameOrConnectionString = "DefaultConnection";
        }
        
        /// <summary>
        /// Método destinado a modificar a string de conexão usada pelo Entity Framework
        /// </summary>
        /// <param name="connection">String de conexão</param>
        public void SetConnection(string nameOrConnectionString)
        {
            if (string.IsNullOrEmpty(nameOrConnectionString))
            {
                nameOrConnectionString =
                    (ConfigurationManager.ConnectionStrings["DefaultConnection"]).ConnectionString;
            }

            this.nameOrConnectionString = nameOrConnectionString;
        }

        /// <summary>
        /// Método auxiliar destinado a incluir referências a classes com propriedades de chasves estrangeiras. 
        /// </summary>
        /// <param name="dbSet">Objeto de Entity Framework o qual permite acesso a uma tabela vinculada a uma Model pelo attribute "TableAttribute"</param>
        /// <param name="navigationProperties">Objetos de uma Model referentes a chaves estrangeiras no database</param>
        /// <returns>Objeto utilizado na query já possuindo a referencia relacional de chave estrangeira</returns>
        private IQueryable<TModel> IncludeReference(DbSet<TModel> dbSet, params Expression<Func<TModel, object>>[] navigationProperties)
        {
            IQueryable<TModel> query = dbSet;

                foreach (Expression<Func<TModel, object>> navigationProperty in navigationProperties)
                    query = query.Include<TModel, object>(navigationProperty);

            return query;
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
            List<TModel> list;

            using (var context = new Repository<TModel>(this.nameOrConnectionString))
            {
                context._dbQuery = IncludeReference(context.DbObject, navigationProperties);

                list = context._dbQuery
                    .AsNoTracking()
                    .ToList<TModel>();
            }
            return list;
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
            List<TModel> list;

            using (var context = new Repository<TModel>(this.nameOrConnectionString))
            {
                context._dbQuery = IncludeReference(context.DbObject, navigationProperties);

                list = context._dbQuery
                    .AsNoTracking()
                    .Where(where)
                    .ToList<TModel>();
            }

            return list;
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
            TModel item = null;

            using (var context = new Repository<TModel>(this.nameOrConnectionString))
            {
                context._dbQuery = IncludeReference(context.DbObject, navigationProperties);

                item = context._dbQuery
                    .AsNoTracking()
                    .FirstOrDefault(where);
            }

            return item;
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a inserir uma coleção de registros.
        /// </summary>
        /// <param name="items">Coleção de registros a inserir na base</param>
        public void Insert(params TModel[] items)
        {
            using (var context = new Repository<TModel>(this.nameOrConnectionString))
            {
                this.InsertToContext(items, context);
            }
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a atualizar uma coleção muitos de registros. Em uma única transaction.
        /// </summary>
        /// <param name="items">Coleção de registros a inserir na base</param>
        public void BulkInsert(params TModel[] items)
        {
            using (var context = new Repository<TModel>(this.nameOrConnectionString))
            using (var transaction = context.Database.BeginTransaction())
            {
                this.InsertToContext(items, context);
                transaction.Commit();
            }
        }

        private void InsertToContext(TModel[] items, Repository<TModel> context)
        {
            foreach (TModel item in items)
            {
                if (item is IBaseModel)
                {
                    var baseItem = (IBaseModel)item;
                    baseItem.Created = DateTime.Now;
                    baseItem.Activated = true;
                }

                context.Entry(item).State = EntityState.Added;
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a atualizar uma coleção de registros.
        /// </summary>
        /// <param name="items">Coleção de registros a inserir na base</param>
        public void Update(params TModel[] items)
        {
            using (var context = new Repository<TModel>(this.nameOrConnectionString))
            {
                this.UpdateToContext(items, context);
            }
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a atualizar uma coleção de registros.
        /// </summary>
        /// <param name="items">Coleção de registros a inserir na base</param>
        public void BulkUpdate(params TModel[] items)
        {
            using (var context = new Repository<TModel>(this.nameOrConnectionString))
            using (var transaction = context.Database.BeginTransaction())
            {
                this.UpdateToContext(items, context);
                transaction.Commit();
            }
        }

        private void UpdateToContext(TModel[] items, Repository<TModel> context)
        {
            foreach (TModel item in items)
            {
                if (item is IBaseModel)
                {
                    ((IBaseModel)item).Updated = DateTime.Now;
                }

                var modelFound = context.Set<TModel>().Local.SingleOrDefault(
                    e => e.Equals(item)
                );

                if (modelFound != null)
                { // Entity exist in the context, then update it by setting the values
                    var attachedEntry = context.Entry(modelFound);
                    attachedEntry.CurrentValues.SetValues(item);
                }
                else
                {
                    context.Entry(item).State = EntityState.Modified; // This should attach entity
                }
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a excluir (logicamente ou fisicamente) uma coleção de registros.
        /// </summary>
        /// <param name="items">Coleção de registros a inserir na base</param>
        public void Delete(params TModel[] items)
        {
            using (var context = new Repository<TModel>(this.nameOrConnectionString))
            {
                foreach (TModel item in items)
                {
                    if (item is IBaseModel)
                    {
                        var baseItem = (IBaseModel)item;
                        baseItem.Updated = DateTime.Now;
                        baseItem.Activated = false;
                        context.Entry(item).State = EntityState.Modified;
                    }
                    else
                    {
                        context.Entry(item).State = EntityState.Deleted;
                    }
                }

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a executar querys customizadas e procedures.
        /// </summary>
        /// <param name="sql">Query ou nome de procedure</param>
        /// <param name="parameters">Dicionaário com os parâmetros e valores a incluir</param>
        /// <param name="cmdType">Tipo de execução: query/procedure</param>
        /// <returns>Implementação de listagem contendo os resultados obtidos</returns>
        public IList<TModel> ExecuteQuery(string sql, Dictionary<string, object> parameters = null, CommandType cmdType = CommandType.Text)
        {
            List<TModel> list = null;
            
            if (!String.IsNullOrEmpty(sql))
            {
                using (var context = new Repository<TModel>(this.nameOrConnectionString))
                {
                    var conn = this.OpenConnection(context);
                    var comm = this.CreateCommand(conn, sql, cmdType);
                    var param = this.SetParameter(comm, parameters);

                    var reader = comm.ExecuteReader();

                    ORM or = new ORM();
                    var props = or.GetProperties(typeof(TModel));
                    list = or.GetModel<TModel>(reader, props);
                }
            }

            return list;
        }


        public IList<TModel> ExecuteQuery(string sql, List<Tuple<ParameterDirection, string, object>> parametersWithDirection, CommandType cmdType)
        {
            List<TModel> list = null;

            if (!String.IsNullOrEmpty(sql))
            {
                using (var context = new Repository<TModel>(this.nameOrConnectionString))
                {
                    var conn = this.OpenConnection(context);
                    var comm = this.CreateCommand(conn, sql, cmdType);
                    var param = this.SetParameter(comm, parametersWithDirection);

                    var reader = comm.ExecuteReader();

                    ORM or = new ORM();
                    var props = or.GetProperties(typeof(TModel));
                    list = or.GetModel<TModel>(reader, props);

                    foreach (DbParameter itemParam in comm.Parameters)
                    {
                        if (itemParam.Direction == ParameterDirection.InputOutput ||
                            itemParam.Direction == ParameterDirection.Output ||
                            itemParam.Direction == ParameterDirection.ReturnValue)
                        {
                            var indexFound = parametersWithDirection.FindIndex(
                                p => p.Item2 == itemParam.ParameterName
                            );

                            if (indexFound >= 0)
                            {
                                parametersWithDirection[indexFound] = new Tuple<ParameterDirection, string, object>(
                                    itemParam.Direction, 
                                    itemParam.ParameterName, 
                                    itemParam.Value
                                );
                            }
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a executar querys customizadas e procedures.
        /// </summary>
        /// <param name="sql">Query ou nome de procedure</param>
        /// <param name="parameters">Dicionaário com os parâmetros e valores a incluir</param>
        /// <param name="cmdType">Tipo de execução: query/procedure</param>
        /// <returns>Objeto contendo o retorno do Scalar</returns>
        public object ExecuteScalar(string sql, Dictionary<string, object> parameters = null, CommandType cmdType = CommandType.Text)
        {
            object result = null;

            if (!String.IsNullOrEmpty(sql))
            {
                using (var context = new Repository<TModel>(this.nameOrConnectionString))
                {
                    var conn = this.OpenConnection(context);
                    var comm = this.CreateCommand(conn, sql, cmdType);
                    var param = this.SetParameter(comm, parameters);
                    
                    result = comm.ExecuteScalar();
                }
            }

            return result;
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a expor o Context de maneira segura para a DAO.
        /// </summary>
        /// <param name="lambda">Delegate para expor o Context e retornar uma Model preenchida</param>
        /// <returns>Objeto de classe modelo preenchido com registro encontrado</returns>
        public TModel Exec(Func<DbSet<TModel>, TModel> lambda)
        {
            TModel model = null;
            if (lambda != null)
            {
                using (var context = new Repository<TModel>(this.nameOrConnectionString))
                {
                    model = lambda(context.DbObject);
                }
            }
            return model;
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a expor o Context de maneira segura para a DAO.
        /// </summary>
        /// <typeparam name="TGenericModel">Tipo de Model de entrada para preencher DbSet</typeparam>
        /// <param name="lambda">Delegate para expor o Context e retornar uma Model preenchida</param>
        /// <returns>Model de retorno preenchida com registro encontrado</returns>
        public TGenericModel Exec<TGenericModel>(Func<DbSet<TGenericModel>, TGenericModel> lambda)
            where TGenericModel : class, new()
        {
            TGenericModel model = null;
            if (lambda != null)
            {
                using (var context = new Repository<TGenericModel>(this.nameOrConnectionString))
	            {
                    model = lambda(context.DbObject);
	            }
            }
            return model;
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a expor o Context de maneira segura para a DAO.
        /// </summary>
        /// <typeparam name="TGenericModel">Tipo de Model de entrada para preencher DbSet</typeparam>
        /// <typeparam name="TGenericModeOut">Tipo de Model de saida</typeparam>
        /// <param name="lambda">Delegate para expor o Context e retornar uma Model preenchida</param>
        /// <returns>Model de retorno preenchida com registro encontrado</returns>
        public TGenericModeOut Exec<TGenericModel, TGenericModeOut>(Func<DbSet<TGenericModel>, TGenericModeOut> lambda)
            where TGenericModel : class, new()
        {
            TGenericModeOut model = default(TGenericModeOut);
            if (lambda != null)
            {
                using (var context = new Repository<TGenericModel>(this.nameOrConnectionString))
                {
                    model = lambda(context.DbObject);
                }
            }
            return model;
        }

        private DbConnection OpenConnection(Repository<TModel> context)
        {
            DbConnection conn = null;

            if (context != null 
                && context.Database != null
                && context.Database.Connection != null
                && context.Database.Connection is DbConnection)
            {
                conn = ((DbConnection)context.Database.Connection);
                conn.Open();
            }
            
            return conn;
        }

        private DbCommand CreateCommand(DbConnection conn, string sql, CommandType cmdType = CommandType.Text)
        {
            DbCommand comm = null;

            if (conn != null 
                && conn.State == ConnectionState.Open
                && !string.IsNullOrEmpty(sql))
            {
                comm = conn.CreateCommand();
                comm.CommandText = sql;
                comm.CommandType = cmdType;
                comm.CommandTimeout = 99999;    
            }
            
            return comm;
        }

        private DbParameter SetParameter(DbCommand comm, Dictionary<string, object> parameters = null)
        {
            DbParameter param = null;

            if (comm != null 
                && parameters != null 
                && parameters.Count > 0)
            {
                foreach (var item in parameters)
                {
                    param = comm.CreateParameter();
                    param.ParameterName = item.Key;
                    param.Value = item.Value;
                    comm.Parameters.Add(param);
                }
            }

            return param;
        }

        private DbParameter SetParameter(DbCommand comm, List<Tuple<ParameterDirection, string, object>> parameters = null)
        {
            DbParameter param = null;

            if (comm != null
                && parameters != null
                && parameters.Count > 0)
            {
                foreach (var item in parameters)
                {
                    param = comm.CreateParameter();
                    param.Direction = item.Item1;
                    param.ParameterName = item.Item2;
                    param.Value = item.Item3;
                    comm.Parameters.Add(param);
                }
            }

            return param;
        }
    }
}