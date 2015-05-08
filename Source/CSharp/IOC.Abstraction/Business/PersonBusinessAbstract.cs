using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.FW.Core;
using IOC.Model;
using IOC.Abstraction.DAO;
using IOC.FW.Core.Base;

namespace IOC.Abstraction.Business
{
    public abstract class PersonBusinessAbstract
        : BaseBusiness<Person>
    {
        public PersonBusinessAbstract(PersonDAOAbstract dao)
            : base(dao)
        { }
    }
}