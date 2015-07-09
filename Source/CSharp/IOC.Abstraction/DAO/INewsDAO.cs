using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.FW.Core;
using IOC.Model;
using IOC.FW.Core.Base;
using IOC.FW.Core.Abstraction.DAO;
using IOC.FW.Core.Database.Repository;

namespace IOC.Abstraction.DAO
{
    public interface INewsDAO
        : IRepository<News>
    {
        bool Test(string Title);
    }
}