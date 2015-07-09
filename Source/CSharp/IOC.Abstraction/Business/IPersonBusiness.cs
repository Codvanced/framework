using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.FW.Core.Abstraction.Business;
using IOC.FW.Core.Abstraction.DAO;
using IOC.FW.Core.Base;
using IOC.Model;

namespace IOC.Abstraction.Business
{
    public interface IPersonBusiness
        : IBaseBusiness<Person>
    {
    }
}