using System.Data.Entity;

namespace IOC.FW.Repository.EF6.Abstraction.Model
{
    /// <summary>
    /// Interface responsável por criar integridade referencial entre entidades do EntityFramework
    /// </summary>
    public interface IModelCreating
    {
        /// <summary>
        /// Método responsável por criar integridade referencial entre entidades do EntityFramework
        /// </summary>
        /// <param name="modelBuilder">Classe do EnetityFramework, utilizada para modelar referencias</param>
        void OnCreating(DbModelBuilder modelBuilder);
    }
}