using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOC.FW.Core.Base
{
    /// <summary>
    /// Classe base para retorno de Model tipado como dynamic
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// Propriedade para indicar o Id da operação
        /// </summary>
        public int IdOp { get; set; }
        /// <summary>
        /// Propriedade para indicar a Mensagem de erro/sucesso
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Propriedade para indicar falha ou sucesso da operação
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Propriedade para indicar dados especificos de uma operação (serialização de tipos)
        /// </summary>
        public dynamic Data { get; set; }
    }

    /// <summary>
    /// Classe base para retorno de Json tipado como GenericType
    /// </summary>
    public class BaseModel<TModel>
    {
        /// <summary>
        /// Propriedade para indicar o Id da operação
        /// </summary>
        public int IdOp { get; set; }
        /// <summary>
        /// Propriedade para indicar a Mensagem de erro/sucesso
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Propriedade para indicar falha ou sucesso da operação
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Propriedade para indicar dados especificos de uma operação (serialização de tipos)
        /// </summary>
        public TModel Data { get; set; }
    }
}