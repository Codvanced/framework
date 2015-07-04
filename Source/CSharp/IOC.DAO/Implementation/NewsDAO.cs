using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.Model;
using IOC.Abstraction.DAO;
using System.Data;

namespace IOC.DAO.Implementation
{
    public class NewsDAO
        : NewsDAOAbstract
    {
        public NewsDAO()
            : base()
        { }

        public override bool Test(string Title)
        {
            IList<News> news = null;
            
            this.ExecuteWithTransaction(
                IsolationLevel.ReadCommitted,
                conn =>
                {
                    var parameters = new Dictionary<string, object>(){
                        { "@Title", Title }
                    };

                    string query = @"SELECT 
                                         *
                                     FROM
                                         News
                                     WHERE
                                         Title = @Title";
                    news = this.ExecuteQuery(query, parameters);
                }
            );

            return
                news != null
                && news.Any();
        }
    }
}