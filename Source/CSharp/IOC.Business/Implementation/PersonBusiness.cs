using IOC.Model;
using IOC.Abstraction.Business;
using IOC.FW.Core.Abstraction.Repository;
using IOC.FW.Core.Implementation.Base;

namespace IOC.Business.Implementation
{
    public class PersonBusiness
        : BaseRepository<Person>, IPersonBusiness
    {
        public PersonBusiness(IRepository<Person> dao)
            : base(dao)
        { }
    }
}