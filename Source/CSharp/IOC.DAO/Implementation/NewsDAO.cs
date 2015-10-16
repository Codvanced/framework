﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.Model;
using IOC.Abstraction.DAO;
using System.Data;
using IOC.FW.Core.Abstraction.Repository;
using System.Data.Common;
using IOC.FW.Core.Implementation.Base;

namespace IOC.DAO.Implementation
{
    public class NewsDAO
        : BaseRepository<News>, INewsDAO
    {
        private readonly IRepository<Ocupation> _ocupationDAO;

        public NewsDAO(IRepository<News> dao, IRepository<Ocupation> ocupationDAO)
            : base(dao)
        {
            _ocupationDAO = ocupationDAO;
        }

        public bool Test(string Title)
        {
            IList<News> news = null;

            this.ExecuteWithTransaction(
                IsolationLevel.Serializable,
                new IBaseTransaction[] { 
                    //_ocupationDAO
                },
                transaction => Execution(transaction, news, Title)
            );

            return news != null
                && news.Any();
        }

        private void Execution(DbTransaction transaction, IList<News> news, string Title)
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
            _ocupationDAO.Insert(new Ocupation
            {
                OcupationName = "Ocupation",
                Activated = true,
                Created = DateTime.Now
            });

            transaction.Rollback();
        }
    }
}