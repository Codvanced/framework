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
using IOC.FW.Core.Abstraction.Miscellaneous;
using System.Reflection;

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
        #region Fields
        /// <summary>
        /// Nome de conexão ou a string propriamente dita
        /// </summary>
        private string nameOrConnectionString = string.Empty;

        /// <summary>
        /// Field responsável por guardar o estado de uma conexão
        /// </summary>
        private DbConnection connection = null;

        /// <summary>
        /// Field responsável por guardar o estado de uma transação
        /// </summary>
        private DbTransaction transaction = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Construtor padrão, inicializa a string de conexão com o parametrizado em ConnectionStrings.config (Name: DefaultConnection)
        /// </summary>
        public BaseRepository()
        {
            //TODO: Modificar a atribuição de nome default para um configurador
            nameOrConnectionString = "DefaultConnection";
        }
        #endregion

        #region Methods
        #region Private Methods
        /// <summary>
        /// Método auxiliar destinado a incluir referências a classes com propriedades de chasves estrangeiras. 
        /// </summary>
        /// <param name="dbSet">Objeto de Entity Framework o qual permite acesso a uma tabela vinculada a uma Model pelo attribute "TableAttribute"</param>
        /// <param name="navigationProperties">Objetos de uma Model referentes a chaves estrangeiras no database</param>
        /// <returns>Objeto utilizado na query já possuindo a referencia relacional de chave estrangeira</returns>
        private IQueryable<TModel> IncludeReference(
            DbSet<TModel> dbSet,
            params Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            IQueryable<TModel> query = dbSet;

            foreach (Expression<Func<TModel, object>> navigationProperty in navigationProperties)
                query = query.Include<TModel, object>(navigationProperty);

            return query;
        }

        /// <summary>
        /// Método responsável por configurar uma transação para todas as DAOs informadas
        /// </summary>
        /// <param name="daos">Objetos de acesso a dados a configurar</param>
        private void ConfigureTransaction(IBaseTransaction[] daos)
        {
            if (daos == null)
            {
                throw new ArgumentNullException("daos");
            }

            foreach (var dao in daos)
            {
                dao.SetConnection(
                    this.connection,
                    this.transaction
                );
            }
        }

        /// <summary>
        /// Método responsável por gerenciar a criação de contextos de repositórios
        /// </summary>
        /// <returns>Contexto de repositório preparado para utilização</returns>
        private Repository<TModel> CreateContext()
        {
            if (this.connection != null
                && this.connection.State == ConnectionState.Open
            )
            {
                var newRepo = new Repository<TModel>(this.connection, false);
                newRepo.Database.UseTransaction(this.transaction);
                return newRepo;
            }
            else
            {
                return new Repository<TModel>(this.nameOrConnectionString);
            }
        }

        /// <summary>
        /// Método auxiliar destinado a abrir uma conexão com o banco e devolvê-la.
        /// </summary>
        /// <param name="context">Contexto do EntityFramework</param>
        /// <returns>Objeto com conexão aberta</returns>
        private DbConnection OpenConnection(Repository<TModel> context)
        {
            DbConnection conn = null;

            if (context != null
                && context.Database != null
                && context.Database.Connection != null
                && context.Database.Connection is DbConnection
                && context.Database.Connection.State != ConnectionState.Open)
            {
                conn = ((DbConnection)context.Database.Connection);
                conn.Open();
            }
            else
            {
                conn = this.connection;
            }

            return conn;
        }

        /// <summary>
        /// Método auxiliar destinado a criar um command baseado em um objeto de connection.
        /// </summary>
        /// <param name="conn">Objeto de conexão aberta</param>
        /// <param name="sql">Comando a ser executado (Query ou procedure)</param>
        /// <param name="cmdType">Tipo do comando</param>
        /// <returns>Objeto de command</returns>
        private DbCommand CreateCommand(
            DbConnection conn,
            string sql,
            CommandType cmdType = CommandType.Text
        )
        {
            if (conn == null)
            {
                throw new ArgumentNullException("conn");
            }

            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException("sql");
            }

            DbCommand comm = null;

            if (conn.State == ConnectionState.Open)
            {
                comm = conn.CreateCommand();
                comm.CommandText = sql;
                comm.CommandType = cmdType;
                comm.CommandTimeout = 99999;
                if (this.transaction != null)
                {
                    comm.Transaction = this.transaction;
                }
            }

            return comm;
        }

        /// <summary>
        /// Método auxiliar destinado a incluir parametros em um command.
        /// </summary>
        /// <param name="comm">Command a inserir os paramtros</param>
        /// <param name="parameters">Dicionario de parametros a inserir</param>
        private void SetParameter(
            DbCommand comm,
            Dictionary<string, object> parameters
        )
        {
            if (comm == null)
            {
                throw new ArgumentNullException("comm");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            if (parameters.Count <= 0)
            {
                throw new ArgumentException("The dictionary can't be empty", "parameters");
            }

            foreach (var item in parameters)
            {
                var param = comm.CreateParameter();
                param.ParameterName = item.Key;
                param.Value = item.Value;
                comm.Parameters.Add(param);
            }
        }

        /// <summary>
        /// Método auxiliar destinado a incluir parametros em um command.
        /// </summary>
        /// <param name="comm">Command a inserir os paramtros</param>
        /// <param name="parameters">Lista de parametros a inserir</param>
        private void SetParameter(
            DbCommand comm,
            List<Tuple<ParameterDirection, string, object>> parameters
        )
        {
            if (comm == null)
            {
                throw new ArgumentNullException("comm");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            if (parameters.Count <= 0)
            {
                throw new ArgumentException("The list of tuples can't be empty", "parameters");
            }

            foreach (var item in parameters)
            {
                var param = comm.CreateParameter();
                param.Direction = item.Item1;
                param.ParameterName = item.Item2;
                param.Value = item.Item3;
                comm.Parameters.Add(param);
            }
        }

        /// <summary>
        /// Implementação de método para retornar um count da tabela vinculada ao objeto
        /// </summary>
        /// <returns>Quantidade de registros</returns>
        private int Count(
            Expression<Func<TModel, bool>> where,
            Repository<TModel> context
        )
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return context._dbQuery
                   .AsNoTracking()
                   .Count(where);
        }

        /// <summary>
        /// Método auxiliar para retornar um count da tabela vinculada ao objeto e contexto
        /// </summary>
        /// <param name="where">Filtro de busca</param>
        /// <param name="context">Contexto de repositorio para a execução da query</param>
        /// <returns>Quantidade de registros</returns>
        private long LongCount(
            Expression<Func<TModel, bool>> where,
            Repository<TModel> context
        )
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return context._dbQuery
                   .AsNoTracking()
                   .LongCount(where);
        }
        #endregion

        #region Public Methods
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
            List<TModel> list;

            using (var context = CreateContext())
            {
                var query = context._dbQuery;
                query = IncludeReference(context.DbObject, navigationProperties);

                if (order != null)
                    query = order(query);

                list = query
                   .AsNoTracking()
                   .ToList<TModel>();
            }
            return list;
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
            return SelectAll(null, navigationProperties);
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
            return Select(where, null, navigationProperties);
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a encontrar todos os registros de uma tabela vinculada a uma model. 
        /// </summary>
        /// <param name="where">Delegate contendo parâmetros para composição de WHERE</param>
        /// <param name="navigationProperties">Objetos de uma Model referentes a chaves estrangeiras no database</param>
        /// <param name="order">Delegate contendo parâmetros de ordenação</param>
        /// <returns>Implementação de IList com os registros encontrados.</returns>
        public IList<TModel> Select(
            Expression<Func<TModel, bool>> where,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> order,
            params Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            List<TModel> list;

            using (var context = CreateContext())
            {
                var query = context._dbQuery;
                query = IncludeReference(context.DbObject, navigationProperties);

                if (order != null)
                    query = order(query);

                list = query
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
            Expression<Func<TModel, bool>> where,
            params Expression<Func<TModel, object>>[] navigationProperties
        )
        {
            TModel item = null;

            using (var context = CreateContext())
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
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            if (items.Length <= 0)
            {
                throw new ArgumentException("The array of itens is empty ", "items");
            }

            using (var context = CreateContext())
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
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a atualizar uma coleção de registros.
        /// </summary>
        /// <param name="items">Coleção de registros a inserir na base</param>
        public void Update(params TModel[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            if (items.Length <= 0)
            {
                throw new ArgumentException("The array of itens is empty ", "items");
            }

            using (var context = CreateContext())
            {
                foreach (TModel item in items)
                {
                    if (item is IBaseModel)
                    {
                        ((IBaseModel)item).Updated = DateTime.Now;
                    }

                    var modelFound = context
                        .Set<TModel>()
                        .Local
                        .SingleOrDefault(
                            e => e.Equals(item)
                        );

                    if (modelFound != null)
                    {
                        var attachedEntry = context.Entry(modelFound);
                        attachedEntry.CurrentValues.SetValues(item);
                    }
                    else
                    {
                        context.Entry<TModel>(item).State = EntityState.Modified;
                    }
                }

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a atualizar propriedades específicas de um objeto.
        /// </summary>
        /// <param name="item">Item a atualizar na base</param>
        /// <param name="properties">Propriedades do objeto a atualizar na base</param>
        public void Update(
            TModel item,
            Expression<Func<TModel, object>>[] properties
        )
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            using (var context = CreateContext())
            {
                if (item is IBaseModel)
                {
                    ((IBaseModel)item).Updated = DateTime.Now;
                }

                var modelFound = context
                    .Set<TModel>()
                    .Local
                    .SingleOrDefault(
                        e => e.Equals(item)
                    );

                if (modelFound != null)
                {
                    var attachedEntry = context.Entry(modelFound);
                    attachedEntry.CurrentValues.SetValues(item);
                }
                else
                {
                    context.DbObject.Attach(item);
                }

                foreach (var property in properties)
                {
                    context.Entry<TModel>(item).Property(property).IsModified = true;
                }

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a excluir (logicamente ou fisicamente) uma coleção de registros.
        /// </summary>
        /// <param name="items">Coleção de registros a inserir na base</param>
        public void Delete(params TModel[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            if (items.Length <= 0)
            {
                throw new ArgumentException("The array of itens is empty ", "items");
            }

            using (var context = CreateContext())
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
        public IList<TModel> ExecuteQuery(
            string sql,
            Dictionary<string, object> parameters = null,
            CommandType cmdType = CommandType.Text
        )
        {
            List<TModel> list = null;

            if (!String.IsNullOrEmpty(sql))
            {
                using (var context = CreateContext())
                {
                    var conn = this.OpenConnection(context);

                    var comm = this.CreateCommand(conn, sql, cmdType);
                    
                    if (parameters != null && parameters.Count > 0)
                    {
                        this.SetParameter(comm, parameters);
                    }

                    var reader = comm.ExecuteReader();

                    ORM or = new ORM();
                    var props = or.GetProperties(typeof(TModel));
                    list = or.GetModel<TModel>(reader, props);
                }
            }

            return list;
        }

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a executar querys customizadas e procedures.
        /// </summary>
        /// <param name="sql">Query ou nome de procedure</param>
        /// <param name="parametersWithDirection">Lista de tupla com a direção dos parâmetros e valores a incluir</param>
        /// <param name="cmdType">Tipo de execução: query/procedure</param>
        /// <returns>Objeto com o resultado obtido</returns>
        public IList<TModel> ExecuteQuery(
            string sql,
            List<Tuple<ParameterDirection, string, object>> parametersWithDirection,
            CommandType cmdType
        )
        {
            List<TModel> list = null;

            if (!String.IsNullOrEmpty(sql))
            {
                using (var context = CreateContext())
                {
                    var conn = this.OpenConnection(context);
                    var comm = this.CreateCommand(conn, sql, cmdType);

                    if (parametersWithDirection != null && parametersWithDirection.Count > 0)
                    {
                        this.SetParameter(comm, parametersWithDirection);
                    }

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
        public object ExecuteScalar(
            string sql,
            Dictionary<string, object> parameters = null,
            CommandType cmdType = CommandType.Text
        )
        {
            object result = null;

            if (!String.IsNullOrEmpty(sql))
            {
                using (var context = CreateContext())
                {
                    var conn = this.OpenConnection(context);
                    var comm = this.CreateCommand(conn, sql, cmdType);

                    if (parameters != null && parameters.Count > 0)
                    {
                        this.SetParameter(comm, parameters);
                    }

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
                using (var context = CreateContext())
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

        /// <summary>
        /// Implementação de método de IBaseTransaction destinado a executar comandos a partir de uma transaction 
        /// </summary>
        /// <param name="isolation">Nível de isolamento para a execução da transaction</param>
        /// <param name="DAOs">Objetos de dados para configurar no mesmo contexto de transaction (todo e qualquer acesso a base através destes objetos será transacional)</param>
        /// <param name="transactionExecution">Método para passar o controle de execução transacional</param>
        public void ExecuteWithTransaction(
            IsolationLevel isolation,
            IBaseTransaction[] DAOs,
            Action<DbTransaction> transactionExecution
        )
        {
            using (var context = CreateContext())
            {
                DbContextTransaction contextTransaction = null;

                try
                {
                    contextTransaction = context.Database.BeginTransaction(isolation);

                    this.connection = context.Database.Connection;
                    this.transaction = contextTransaction.UnderlyingTransaction;

                    ConfigureTransaction(DAOs);
                    transactionExecution(this.transaction);

                    if (this.transaction.Connection != null)
                    {
                        contextTransaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (contextTransaction != null 
                        && this.transaction.Connection != null)
                    {
                        contextTransaction.Rollback();
                    }

                    throw ex;
                }

                this.connection = null;
                this.transaction = null;
            }
        }

        /// <summary>
        /// Implementacao de método para atualizar a prioridade do elemento na tabela
        /// </summary>
        /// <typeparam name="TPriorityModel">Tipo do model que implementa IPrioritySortable</typeparam>
        /// <param name="items">Lista de models que implementam IPrioritySortable</param>
        public void UpdatePriority<TPriorityModel>(TPriorityModel[] items)
            where TPriorityModel : TModel, IPrioritySortable
        {
            using (var context = CreateContext())
            {
                for (var i = 0; i < items.Length; i++)
                {
                    var item = items[i];
                    item.Priority = Int64.MaxValue - i;

                    context.DbObject.Attach(item);
                    context.Entry<TModel>(item)
                        .Property("Priority")
                        .IsModified = true;
                }

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Implementacao de método para devolver um objeto de model
        /// </summary>
        /// <returns>Retorna novo objeto</returns>
        public TModel Model()
        {
            return new TModel();
        }

        /// <summary>
        /// Implementacao de método para devolver uma listade de model
        /// </summary>
        /// <returns>Retorna nova lista</returns>
        public List<TModel> List()
        {
            return new List<TModel>();
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
            int count;
            using (var context = CreateContext())
            {
                count = this.Count(where, context);
            }

            return count;
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
            long count;
            using (var context = CreateContext())
            {
                count = this.LongCount(where, context);
            }

            return count;
        }

        /// <summary>
        /// Implementação de método de IBaseTransaction responsável por atribuir uma string de conexão a um repositório 
        /// </summary>
        /// <param name="nameOrConnectionString">String de conexão</param>
        public void SetConnection(string nameOrConnectionString)
        {
            if (string.IsNullOrEmpty(nameOrConnectionString))
            {
                nameOrConnectionString = (ConfigurationManager.ConnectionStrings["DefaultConnection"]).ConnectionString;
            }

            this.nameOrConnectionString = nameOrConnectionString;
        }

        /// <summary>
        /// Implementação de método de IBaseTransaction responsável por associar uma transação a um repositório
        /// </summary>
        /// <param name="connection">Objeto de conexão que criou a transação</param>
        /// <param name="transaction">Objeto de transação aberta</param>
        public void SetConnection(DbConnection connection, DbTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;
        }
        #endregion
        #endregion
    }
}