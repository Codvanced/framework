using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.FW.Core.Abstraction.Business;
using IOC.FW.Core.Abstraction.Repository;
using IOC.Model;

namespace IOC.Abstraction.DAO
{
    public interface IPersonDAO
        : IRepository<Person>
    {
    }
}