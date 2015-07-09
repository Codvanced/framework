using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.FW.Core;
using IOC.Model;
using IOC.Abstraction.DAO;
using IOC.FW.Core.Base;
using IOC.FW.Core.Abstraction.Business;

namespace IOC.Abstraction.Business
{
    public interface INewsBusiness
        : IBaseBusiness<News>
    {
        bool TitleAlreadyExists(string Title, int? id = null);
        bool Test(string Title);
    }
}
