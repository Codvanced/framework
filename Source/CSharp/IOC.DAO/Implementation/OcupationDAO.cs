using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.Abstraction.DAO;
using IOC.FW.Core;
using IOC.FW.Core.Abstraction.Repository;
using IOC.Model;
using IOC.FW.Core.Implementation.Base;

namespace IOC.DAO.Implementation
{
    public class OcupationDAO
        : BaseRepository<Ocupation>, IOcupationDAO
    {
        public OcupationDAO(IRepository<Ocupation> dao)
            : base(dao)
        {
        }
    }
}