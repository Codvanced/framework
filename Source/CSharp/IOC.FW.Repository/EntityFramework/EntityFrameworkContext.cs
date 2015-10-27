using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Configuration;
using IOC.FW.Core.Abstraction.Model;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using IOC.FW.Core.Abstraction.Repository;

namespace IOC.FW.Repository.EntityFramework
{
    /// <summary>
    /// Classe utilizada como interface entre a aplicação e o database, é utilizada como UnitOfWork de DbContext e desabilita a criação automatica de tabelas.
    /// </summary>
    /// <typeparam name="TModel">Tipo que representa a classe modelo referente a uma tabela do database</typeparam>
    internal class EntityFrameworkContext<TModel>
        : DbContext, IContext<TModel>
        where TModel : class, new()
    {
        /// <summary>
        /// Objeto de DbSet para acessar as funções de model do Entity Framework
        /// </summary>
        public virtual DbSet<TModel> DbObject { get; set; }

        /// <summary>
        /// Objeto de IQuereyable, utilizado para incluir objetos de foreign key em querys do Entity Framework
        /// </summary>
        public IQueryable<TModel> DbQuery { get; set; }

        public EntityFrameworkContext()
            : base()
        {
            Setup();
        }

        /// <summary>
        /// Constructor padrão, iniciando o Entity Framework com uma string de conexão passada por parametro
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        public EntityFrameworkContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Setup();
        }

        public EntityFrameworkContext(DbConnection connection, bool destroyAfterUse)
            : base(connection, destroyAfterUse)
        {
            Setup();
        }

        private void Setup()
        {
            System.Data.Entity.Database.SetInitializer<EntityFrameworkContext<TModel>>(null);
            this.DbQuery = this.DbObject;
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Database.CommandTimeout = 99999;
        }

        /// <summary>
        /// Método responsável por permitir que classes informem relacionamentos de base
        /// </summary>
        /// <param name="modelBuilder">Objeto modelador de relacionamentos do Entity</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            TModel model = new TModel();
            if (model is IModelCreating)
            {
                ((IModelCreating)model).OnCreating(modelBuilder);
                base.OnModelCreating(modelBuilder);
            }
        }

        public virtual void SetState(TModel entity, EntityState state)
        {
            this.Entry(entity).State = state;
        }
    }
}