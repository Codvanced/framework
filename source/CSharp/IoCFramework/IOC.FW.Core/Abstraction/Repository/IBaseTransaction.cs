using System;
using System.Data;
using System.Data.Common;
using IOC.FW.Core.Abstraction.Repository.Connection;


namespace IOC.FW.Core.Abstraction.Repository
{
    /// <summary>
    /// Interface responsável por padronizar a utilização de transactions em DAOs
    /// </summary>
    public interface IBaseTransaction
        : IConnectionConfigurable
    {
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
