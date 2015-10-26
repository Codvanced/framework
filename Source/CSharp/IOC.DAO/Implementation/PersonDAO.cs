using IOC.Model;
using IOC.Abstraction.DAO;
using IOC.FW.Core.Implementation.Base;
using IOC.FW.Core.Abstraction.Repository;

namespace IOC.DAO.Implementation
{
    public class PersonDAO
        : BaseRepository<Person>, IPersonDAO
    {
        public PersonDAO(IRepository<Person> dao)
            : base(dao)
        { }
    }
}