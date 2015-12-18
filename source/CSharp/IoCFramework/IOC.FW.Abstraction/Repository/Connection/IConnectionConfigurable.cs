using System.Data.Common;

namespace IOC.FW.Abstraction.Repository.Connection
{
    public interface IConnectionConfigurable
    {
        /// <summary>
        /// Implementação de método de IBaseTransaction responsável por atribuir uma string de conexão a um repositório 
        /// </summary>
        /// <param name="nameOrConnectionString">Nome ou string de conexão</param>
        void SetConnection(string nameOrConnectionString);

        /// <summary>
        /// Implementação de método de IBaseTransaction responsável por associar uma transação a um repositório
        /// </summary>
        /// <param name="connection">Objeto de conexão que criou a transação</param>
        /// <param name="transaction">Objeto de transação aberta</param>
        void SetConnection(DbConnection connection, DbTransaction transaction);
    }
}