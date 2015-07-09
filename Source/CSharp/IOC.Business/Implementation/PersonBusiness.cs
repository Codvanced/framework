using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.FW.Core;
using IOC.Model;
using IOC.Abstraction.Business;
using IOC.Abstraction.DAO;
using IOC.FW.Core.Base;
using IOC.FW.Core.Abstraction.DAO;

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