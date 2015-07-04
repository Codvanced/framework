using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.Abstraction.Business;
using IOC.Abstraction.DAO;

namespace IOC.Business.Implementation
{
    public class OcupationBusiness
        : OcupationBusinessAbstract
    {
        public OcupationBusiness(OcupationDAOAbstract dao)
            : base(dao)
        { }
    }
}
