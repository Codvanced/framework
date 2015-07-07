using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;


namespace IOC.FW.Core.Abstraction.DAO
{
    /// <summary>
    /// Interface responsável por padronizar a utilização de transactions em DAOs
    /// </summary>
    public interface IBaseTransaction
    {
        /// <summary>
        /// Implementação de método de IBaseTransaction responsável por associar uma transação a um repositório
        /// </summary>
        /// <param name="connection">Objeto de conexão que criou a transação</param>
        /// <param name="transaction">Objeto de transação aberta</param>
        void SetConnection(DbConnection connection, DbTransaction transaction);

        /// <summary>
        /// Implementação de método de IBaseTransaction responsável por atribuir uma string de conexão a um repositório 
        /// </summary>
        /// <param name="nameOrConnectionString">Nome ou string de conexão</param>
        void SetConnection(string nameOrConnectionString);

        /// <summary>
        /// Implementação de método de IBaseDAO destinado a executar comandos a partir de uma transaction 
        /// </summary>
        /// <param name="isolation">Nível de isolamento para a execução da transaction</param>
        /// <param name="DAOs">Objetos de dados para configurar no mesmo contexto de transaction (todo e qualquer acesso a base através destes objetos será transacional)</param>
        /// <param name="transactionExecution">Método para passar o controle de execução transacional</param>
        void ExecuteWithTransaction(
            IsolationLevel isolation,
            IBaseTransaction[] DAOs,
            Action<DbTransaction> transactionExecution
        );
    }
}
