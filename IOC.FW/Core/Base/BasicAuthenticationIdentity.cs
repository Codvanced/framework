using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace IOC.FW.Core.Base
{
    public class BasicAuthenticationIdentity
        : GenericIdentity
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public BasicAuthenticationIdentity(string name, string password)
            : base(name, "Basic")
        {
            this.Password = password;
        }
    }
}