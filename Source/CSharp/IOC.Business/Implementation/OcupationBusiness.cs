using IOC.Abstraction.Business;
using IOC.Abstraction.DAO;
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