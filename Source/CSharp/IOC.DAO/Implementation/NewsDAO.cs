using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.Model;
using System.Data.SqlClient;
using IOC.Abstraction.DAO;
using IOC.Interface.DAO;

namespace IOC.DAO.Implementation
{
    public class NewsDAO
        : NewsDAOAbstract
    {
        public NewsDAO()
            : base()
        {
        }

        public override bool Test(string Title)
        {
            var parameters = new Dictionary<string, object>(){
                { "@Title", Title }
            };

            string query = @"SELECT * FROM News WHERE Title = @Title";
            var news = this.ExecuteQuery(query, parameters);

            int contador = this.Exec<News, int>(
                p => 
                { 
                    return p.Count(); 
                }
            );

            IList<News> listNews = this.SelectAll();

            var whatever = (
                from obj in listNews 
                where obj.IdNews == 1 select obj
            );

            var whatever2 = listNews
                .Where(n => n.IdNews == 1)
                .Select(p => p);

            var whatever3 = (
                from obj in listNews 
                where obj.IdNews == 1 
                select new 
                { 
                    ID = obj.IdNews, 
                    Name = obj.Author 
                }
            ).ToList();


            var whatever4 = listNews
                .Where(n => n.IdNews == 1)
                .Select(
                    p => new 
                    { 
                        ID = p.IdNews, 
                        Name = p.Author 
                    }
                ).ToList();

            return
                news != null
                && news.Any();
        }
    }
}
