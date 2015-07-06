using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleInjector;
using IOC.FW.Core.Factory;
using IOC.FW.Core.Abstraction.Business;
using IOC.Model;
using System.Linq.Expressions;
using IOC.FW.Core.Database;
using System.Data.Entity;
using System.Collections;
using NUnit.Framework;
using Moq;
using IOC.FW.Core.Abstraction.DAO;
using IOC.FW.Core.Base;

namespace IOC.Test
{
    [TestFixture]
    public class IBaseBusinessTest
    {
        [Test]
        public void create_model_test()
        {
            var business = InstanceFactory.GetImplementation<IBaseBusiness<News>>();
            var model = business.Model();
            Assert.IsNotNull(model);
        }

        [Test]
        public void create_list_model_test()
        {
            var business = InstanceFactory.GetImplementation<IBaseBusiness<News>>();
            var model = business.List();
            Assert.IsNotNull(business);
            Assert.IsNotNull(model);
        }

        [Test]
        public void insert_model_test()
        {
            var business = InstanceFactory.GetImplementation<IBaseBusiness<News>>();
            var model = business.Model();

            model.Author = "Autor";
            model.Title = "Titulo";
            model.NewsDate = DateTime.Now;
            model.NewsDescription = "Descrição";
            model.Priority = long.MaxValue;
            business.Insert(model);

            Assert.True(model.IdNews > 0);
        }

        [Test]
        public void select_model_with_where_clause_test()
        {
            var business = InstanceFactory.GetImplementation<IBaseBusiness<News>>();
            var model = business.Model();

            model.Author = "Autor";
            model.Title = "Titulo";
            model.NewsDate = DateTime.Now;
            model.NewsDescription = "Descrição";
            model.Priority = long.MaxValue;
            business.Insert(model);

            var found = business.Select(w => w.IdNews == model.IdNews);
            Assert.True(model.IdNews > 0);
            Assert.IsNotNull(found);
            Assert.IsNotEmpty(found);
        }

        [Test]
        public void select_model_with_where_and_orderby_clause_test()
        {
            var business = InstanceFactory.GetImplementation<IBaseBusiness<News>>();
            var model = business.Model();

            for (int i = 0; i < 3; i++)
            {
                model.Author = string.Format("Autor orderby {0}", i);
                model.Title = string.Format("Titulo {0}", i);
                model.NewsDate = DateTime.Now;
                model.NewsDescription = string.Format("Descrição {0}", i);
                model.Priority = long.MaxValue;
                business.Insert(model);
            }

            var ordered = business.Select(
                where => where.Author.Contains("orderby"),
                new Func<IQueryable<News>, IOrderedQueryable<News>>(
                    p => p.OrderBy(o => o.Author)
                )
            );

            business.Delete(ordered.ToArray());

            Assert.IsNotEmpty(ordered);
            Assert.IsTrue(ordered.Count == 3);
        }

        [Test]
        public void select_all_model_test()
        {
            var business = InstanceFactory.GetImplementation<IBaseBusiness<News>>();
            var model = business.Model();

            model.Author = "Autor";
            model.Title = "Titulo";
            model.NewsDate = DateTime.Now;
            model.NewsDescription = "Descrição";
            model.Priority = long.MaxValue;
            business.Insert(model);

            var found = business.SelectAll();
            Assert.True(model.IdNews > 0);
            Assert.IsNotNull(found);
            Assert.IsNotEmpty(found);
        }

        [Test]
        public void select_all_model_with_orderby_clause_test()
        {
            var business = InstanceFactory.GetImplementation<IBaseBusiness<News>>();
            var model = business.Model();

            model.Author = "Autor";
            model.Title = "Titulo";
            model.NewsDate = DateTime.Now;
            model.NewsDescription = "Descrição";
            model.Priority = long.MaxValue;
            business.Insert(model);

            var found = business.SelectAll(
                new Func<IQueryable<News>, IOrderedQueryable<News>>(
                    p => p.OrderBy(o => o.Author)
                )
            );

            Assert.True(model.IdNews > 0);
            Assert.IsNotNull(found);
            Assert.IsNotEmpty(found);
        }

        [Test]
        public void update_model_test()
        {
            var business = InstanceFactory.GetImplementation<IBaseBusiness<News>>();
            var model = business.Model();
            model.Author = "Autor";
            model.Title = "Titulo";
            model.NewsDate = DateTime.Now;
            model.NewsDescription = "Descrição";
            model.Priority = long.MaxValue;
            business.Insert(model);

            model.NewsDescription = "has changed!!";
            business.Update(model);

            var found = business.SelectSingle(
                w => w.IdNews == model.IdNews
            );

            Assert.IsNotNull(found);
            Assert.AreEqual(model.NewsDescription, found.NewsDescription);
        }

        [Test]
        public void update_specific_properties_model_test()
        {
            var business = InstanceFactory.GetImplementation<IBaseBusiness<News>>();
            var model = business.Model();
            model.Author = "Autor AAA";
            model.Title = "Titulo AAA";
            model.NewsDate = DateTime.Now;
            model.NewsDescription = "Descrição AAA";
            model.Priority = long.MaxValue;
            business.Insert(model);

            model.NewsDescription = "has changed!!";
            model.Author = "has changed!!";
            model.Title = "has changed!!";

            business.Update(
                item: model, 
                properties: 
                    new Expression<Func<News,object>>[]{  
                        p => p.NewsDescription, 
                        p => p.Updated
                    }
            );

            var found = business.SelectSingle(
                w => w.IdNews == model.IdNews
            );

            Assert.IsNotNull(found);
            Assert.AreEqual(model.NewsDescription, found.NewsDescription);
        }

        [Test]
        public void delete_model_test()
        {
            var business = InstanceFactory.GetImplementation<IBaseBusiness<News>>();
            var model = business.Model();
            model.Author = "Autor";
            model.Title = "Titulo";
            model.NewsDate = DateTime.Now;
            model.NewsDescription = "Descrição";
            model.Priority = long.MaxValue;
            business.Insert(model);

            business.Delete(model);
            var found = business.SelectSingle(
                w => w.IdNews == model.IdNews
            );

            Assert.True(model.IdNews > 0);
            Assert.IsNull(found);
        }

        [Test]
        public void count_model_test()
        {
            var business = InstanceFactory.GetImplementation<IBaseBusiness<News>>();
            var count = business.Count();

            Assert.True(count > 0);
        }

        [Test]
        public void long_count_model_test()
        {
            var business = InstanceFactory.GetImplementation<IBaseBusiness<News>>();
            var count = business.LongCount();

            Assert.True(count > 0);
        }

        [Test]
        public void update_priority_test()
        {
            var business = InstanceFactory.GetImplementation<IBaseBusiness<News>>();
            var all = business.SelectAll();

            business.UpdatePriority<News>(
                all
                .OrderByDescending(p => p.IdNews)
                .ToArray()
            );

            Assert.True(
                all.Any(
                    p => p.Priority != long.MaxValue
                )
            );
        }
    }
}
