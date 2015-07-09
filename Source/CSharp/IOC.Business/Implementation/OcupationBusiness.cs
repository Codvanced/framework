using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.Abstraction.Business;
using IOC.Abstraction.DAO;
using IOC.FW.Core.Abstraction.DAO;
using IOC.FW.Core.Base;
using IOC.Model;

namespace IOC.Business.Implementation
{
    public class OcupationBusiness
        : BaseBusiness<Ocupation>, IOcupationBusiness
    {
        public OcupationBusiness(IRepository<Ocupation> dao)
            : base(dao)
        { }
    }
}
