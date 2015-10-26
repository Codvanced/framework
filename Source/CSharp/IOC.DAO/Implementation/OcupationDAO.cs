using IOC.Abstraction.DAO;
using IOC.FW.Core.Abstraction.Repository;
using IOC.FW.Core.Implementation.Base;
using IOC.Model;

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