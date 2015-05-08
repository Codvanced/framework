using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.Model;
using System.Data.SqlClient;
using IOC.FW.Core.Factory;
using IOC.Abstraction.Business;
using IOC.Abstraction.DAO;

namespace IOC.Business.Implementation
{
    public class NewsBusiness
        : NewsBusinessAbstract
    {
        private readonly NewsDAOAbstract _dao;

        public NewsBusiness(NewsDAOAbstract dao)
            : base(dao)
        {
            _dao = dao;
        }

        public override bool TitleAlreadyExists(string Title, int? id)
        {
            IList<News> newsFound = new List<News>();

            if (id.HasValue && id.Value > 0)
            {
                newsFound = this.Select(n =>
                    n.Title.Equals(Title, StringComparison.OrdinalIgnoreCase)
                    && n.IdNews != id.Value
                );
            }
            else
            {
                newsFound = this.Select(n =>
                    n.Title.Equals(Title, StringComparison.OrdinalIgnoreCase)
                );
            }

            return
                newsFound != null
                && newsFound.Any();
        }

        public override bool Test(string Title)
        {
            return _dao.Test(Title);
        }
    }
}