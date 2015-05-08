using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.FW.Core.Base;
using IOC.Model;
using IOC.Interface.DAO;

namespace IOC.Interface.Business
{
    public abstract class OcupationBusinessAbstract
        : BaseBusiness<Ocupation>
    {
        public OcupationBusinessAbstract(OcupationDAOAbstract dao)
            : base(dao)
        { }
    }
}
