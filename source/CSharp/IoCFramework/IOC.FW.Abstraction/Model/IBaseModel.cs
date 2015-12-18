using System;

namespace IOC.FW.Abstraction.Model
{
    /// <summary>
    /// Interface padrão para models que desejam utilizar data de inclusão, alteração e status de ativação
    /// </summary>
    public interface IBaseModel
    {
        /// <summary>
        /// Propriedade contendo a data de criação do registro
        /// </summary>
        DateTime Created { get; set; }
        
        /// <summary>
        /// Propriedade contendo a data de atualização do registro
        /// </summary>
        DateTime? Updated { get; set; }

        /// <summary>
        /// Propriedade indicando se o registro está ativo
        /// </summary>
        Boolean Activated { get; set; }
    }
}
