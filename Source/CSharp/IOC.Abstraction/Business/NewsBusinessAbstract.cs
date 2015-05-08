using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.FW.Core;
using IOC.Model;
using IOC.Abstraction.DAO;
using IOC.FW.Core.Base;

namespace IOC.Abstraction.Business
{
    public abstract class NewsBusinessAbstract
        : BaseBusiness<News>
    {
        public NewsBusinessAbstract(NewsDAOAbstract dao)
            : base(dao)
        { }

        public abstract bool TitleAlreadyExists(string Title, int? id = null);
        public abstract bool Test(string Title);
    }
}
