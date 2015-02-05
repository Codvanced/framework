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
    public abstract class AbstractPersonBusiness
        : BaseBusiness<Person>
    {
        public AbstractPersonBusiness(AbstractPersonDAO dao)
            : base(dao)
        { }
    }
}