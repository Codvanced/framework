using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.Abstraction.Business;
using IOC.Abstraction.DAO;
using IOC.FW.Core;
using IOC.FW.Core.Abstraction.Business;
using IOC.FW.Core.Abstraction.Repository;
using IOC.Model;
using IOC.FW.Core.Implementation.Base;

namespace IOC.Business.Implementation
{
    public class OcupationBusiness
        : BaseBusiness<Ocupation>, IOcupationBusiness
    {
        public OcupationBusiness(IOcupationDAO dao)
            : base(dao)
        {
            
        }
    }
}