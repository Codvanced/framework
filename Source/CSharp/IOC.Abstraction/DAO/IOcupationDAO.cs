using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.Model;
using IOC.FW.Core.Abstraction.Repository;

namespace IOC.Abstraction.DAO
{
    public interface IOcupationDAO
        : IRepository<Ocupation>
    {
    }
}