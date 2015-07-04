using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.FW.Core.Base;
using IOC.Model;
using IOC.Abstraction.DAO;

namespace IOC.Abstraction.Business
{
    public abstract class OcupationBusinessAbstract
        : BaseBusiness<Ocupation>
    {
        public OcupationBusinessAbstract(OcupationDAOAbstract dao)
            : base(dao)
        { }
    }
}
