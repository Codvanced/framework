using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.FW.Core;
using IOC.Model;
using IOC.FW.Core.Base;

namespace IOC.Abstraction.DAO
{
    public abstract class AbstractPersonDAO
        : BaseRepository<Person>
    {
        public AbstractPersonDAO(string connectionString = null)
            : base()
        { }
    }
}
