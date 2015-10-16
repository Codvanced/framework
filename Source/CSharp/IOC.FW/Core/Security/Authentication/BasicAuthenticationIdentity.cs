using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace IOC.FW.Core.Security.Authentication
{
    /// <summary>
    /// Extensão de classe GenericAuthentication destinada a implementar BasicAuhtentication (RESTful)
    /// </summary>
    public class BasicAuthenticationIdentity
        : GenericIdentity
    {
        /// <summary>
        /// Identificação
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// USuário
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Senha
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Construtor passando parâmetros obrigatorios.
        /// </summary>
        /// <param name="name">Usuário</param>
        /// <param name="password">Senha</param>
        public BasicAuthenticationIdentity(string name, string password)
            : base(name, "Basic")
        {
            this.Password = password;
        }
    }
}