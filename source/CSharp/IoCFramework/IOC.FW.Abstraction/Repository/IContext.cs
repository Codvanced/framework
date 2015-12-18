using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace IOC.FW.Abstraction.Repository
{
    public interface IContext<TModel>
        : IDisposable
        where TModel : class, new()
    {
        DbSet<TModel> DbObject { get; set; }

        IQueryable<TModel> DbQuery { get; set; }

        void SetState(TModel entity, EntityState state);

        DbChangeTracker ChangeTracker { get; }
        
        DbContextConfiguration Configuration { get; }

        System.Data.Entity.Database Database { get; }
        
        DbEntityEntry Entry(object entity);
        
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        
        Type GetType();
        
        IEnumerable<DbEntityValidationResult> GetValidationErrors();
        
        int SaveChanges();
        
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbSet Set(Type entityType);
    }
}