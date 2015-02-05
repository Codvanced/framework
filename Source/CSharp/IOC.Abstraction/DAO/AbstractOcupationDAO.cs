using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.Model;
using IOC.FW.Core.Base;

namespace IOC.Interface.DAO
{
    public abstract class AbstractOcupationDAO
        : BaseRepository<Ocupation>
    {
        public AbstractOcupationDAO(string connectionString = null)
            : base()
        { }
    }
}
