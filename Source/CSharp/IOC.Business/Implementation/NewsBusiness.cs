using System;
using System.Collections.Generic;
using System.Linq;
using IOC.Model;
using IOC.Abstraction.Business;
using IOC.Abstraction.DAO;
using IOC.FW.Core.Implementation.Base;

namespace IOC.Business.Implementation
{
    public class NewsBusiness
        : BaseBusiness<News>, INewsBusiness
    {
        private readonly INewsDAO _dao;

        public NewsBusiness(INewsDAO dao)
            : base(dao)
        {
            _dao = dao;
        }

        public bool TitleAlreadyExists(string Title, int? id)
        {
            IList<News> newsFound = new List<News>();

            if (id.HasValue && id.Value > 0)
            {
                newsFound = Select(n =>
                    n.Title.Equals(Title, StringComparison.OrdinalIgnoreCase)
                    && n.IdNews != id.Value
                );
            }
            else
            {
                newsFound = Select(n =>
                    n.Title.Equals(Title, StringComparison.OrdinalIgnoreCase)
                );
            }

            return
                newsFound != null
                && newsFound.Any();
        }

        public bool Test(string Title)
        {
            return _dao.Test(Title);
        }
    }
}