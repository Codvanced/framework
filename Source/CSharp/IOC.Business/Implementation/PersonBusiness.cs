using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.FW.Core;
using IOC.Model;
using IOC.Abstraction.Business;
using IOC.Abstraction.DAO;
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