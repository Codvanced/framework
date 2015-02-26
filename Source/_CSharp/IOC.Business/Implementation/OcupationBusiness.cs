using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.Interface.Business;
using IOC.Interface.DAO;

namespace IOC.Business.Implementation
{
    public class OcupationBusiness
        : AbstractOcupationBusiness
    {
        public OcupationBusiness(AbstractOcupationDAO dao)
            : base(dao)
        { }
    }
}
