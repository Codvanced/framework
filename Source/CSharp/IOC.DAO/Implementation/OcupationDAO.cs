using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.Abstraction.DAO;
using IOC.FW.Core.Abstraction.DAO;
using IOC.FW.Core.Base;
using IOC.Model;

namespace IOC.DAO.Implementation
{
    public class OcupationDAO
        : BaseRepository<Ocupation>, IOcupationDAO
    {
        public OcupationDAO(IRepository<Ocupation> dao)
            : base(dao)
        { }
    }
}
