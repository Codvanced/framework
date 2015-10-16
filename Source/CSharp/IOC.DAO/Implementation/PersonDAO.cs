using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.Model;
using IOC.FW.Core;
using IOC.Abstraction.DAO;
using IOC.FW.Core.Abstraction.Repository;
using IOC.FW.Core.Implementation.Base;

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