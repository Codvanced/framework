using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.Model;
using IOC.FW.Core.Base;
using IOC.FW.Core.Database.Repository;
using IOC.FW.Core.Abstraction.DAO;

namespace IOC.Abstraction.DAO
{
    public interface IOcupationDAO
        : IRepository<Ocupation>
    {
    }
}