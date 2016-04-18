using IOC.FW.Abstraction.Repository;
using IOC.FW.Repository.EntityFramework;

using Xunit;
using FakeItEasy;

using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using IOC.FW.Shared.Model.Repository;

namespace IOC.FW.Repository.Tests
{
    public class EntityFrameworkRepositoryTests
    {
        private readonly IQueryable<Model> _modelsList;
        private readonly IContext<Model> _context;
        private readonly IContextFactory<Model> _contextFactory;
        private readonly DbSet<Model> _dbSet;
        private readonly ExpressionAppender<Model> _expressionHelper;

        public EntityFrameworkRepositoryTests()
        {
            _expressionHelper = new ExpressionAppender<Model>();
            _modelsList = new List<Model>
            {
                new Model { Id = 1, Name = "Test" },
                new Model { Id = 2, Name = "Test 2" },
                new Model { Id = 3, Name = "Test 3" },
                new Model { Id = 4, Name = "Test 4" }
            }
            .AsQueryable();

            _dbSet = A.Fake<DbSet<Model>>(
                options => options.Implements(typeof(IQueryable<Model>))
            );
            _context = A.Fake<IContext<Model>>();
            _contextFactory = A.Fake<IContextFactory<Model>>();

            A.CallTo(
                () => ((IQueryable<Model>)_dbSet).Provider
            ).Returns(_modelsList.Provider);

            A.CallTo(
                () => ((IQueryable<Model>)_dbSet).Expression
            ).Returns(_modelsList.Expression);

            A.CallTo(
                () => ((IQueryable<Model>)_dbSet).ElementType
            ).Returns(_modelsList.ElementType);

            A.CallTo(
                () => ((IQueryable<Model>)_dbSet).GetEnumerator()
            ).Returns(_modelsList.GetEnumerator());

            A.CallTo(
                () => _context.DbObject
            ).Returns(_dbSet);

            A.CallTo(
                () => _context.DbQuery
            ).Returns(_modelsList);

            A.CallTo(
               () => _contextFactory.GetContext(A<string>.Ignored)
            ).Returns(_context);
        }

        [Fact(DisplayName = "[EF_REP] : Should order by descending when call SelectSingle")]
        public void Should_order_by_descending_when_call_SelectSingle()
        {
            //arrange 
            var repository = new EntityFrameworkRepository<Model>(_contextFactory);

            //act
            var onlyOneItem = repository.SelectSingle(
                order: p => p.OrderByDescending(o => o.Id)
            );

            //assert
            Assert.Equal(onlyOneItem.Id, _modelsList.Last().Id);
        }

        [Fact(DisplayName = "[EF_REP] : Should order by descending and navigate when call SelectSingle")]
        public void Should_order_by_descending_and_navigate_when_call_SelectSingle()
        {
            //arrange 
            var repository = new EntityFrameworkRepository<Model>(_contextFactory);

            //act
            var onlyOneItem = repository.SelectSingle(
                order: p => p.OrderByDescending(o => o.Id),
                navigationProperties: _expressionHelper
                    .Add(e => e.Id)
                    .Evaluate()
            );

            //assert
            Assert.Equal(onlyOneItem.Id, _modelsList.Last().Id);
        }

        [Fact(DisplayName = "[EF_REP] : Should filter by name and order by ascending when call SelectSingle")]
        public void Should_filter_by_name_and_order_by_ascending_when_call_SelectSingle()
        {
            //arrange 
            var repository = new EntityFrameworkRepository<Model>(_contextFactory);

            //act
            var onlyOneItem = repository.SelectSingle(
                where: w => w.Name.Contains("Test "),
                order: p => p.OrderBy(o => o.Id)
            );

            //assert
            Assert.Equal(onlyOneItem.Id, _modelsList.ToArray()[1].Id);
        }

        [Fact(DisplayName = "[EF_REP] : Should filter by name and order by ascending when call SelectSingle")]
        public void Should_filter_by_name_adding_navigation_and_order_by_ascending_when_call_SelectSingle()
        {
            //arrange 
            var repository = new EntityFrameworkRepository<Model>(_contextFactory);

            //act
            var onlyOneItem = repository.SelectSingle(
                where: w => w.Name.Contains("Test "),
                order: p => p.OrderBy(o => o.Id),
                navigationProperties: _expressionHelper
                    .Add(e => e.Id)
                    .Evaluate()
            );

            //assert
            Assert.Equal(onlyOneItem.Id, _modelsList.ToArray()[1].Id);
        }

        [Fact(DisplayName = "[EF_REP] : Should filter by id and order by descending when call Select")]
        public void Should_filter_by_id_and_order_by_descending_when_call_Select()
        {
            //arrange 
            var repository = new EntityFrameworkRepository<Model>(_contextFactory);

            //act
            var onlyOneItem = repository.Select(
                where: w => w.Id == 1,
                order: p => p.OrderByDescending(o => o.Id)
            );

            //assert
            Assert.Equal(onlyOneItem.Count, 1);
        }

        [Fact(DisplayName = "[EF_REP] : Should filter by id and order by descending when call Select")]
        public void Should_filter_by_id_when_call_Select()
        {
            //arrange 
            var repository = new EntityFrameworkRepository<Model>(_contextFactory);

            //act
            var onlyOneItem = repository.Select(
                where: w => w.Id == 1
            );

            //assert
            Assert.Equal(onlyOneItem.Count, 1);
        }

        [Fact(DisplayName = "[EF_REP] : Should order by id when call SelectAll")]
        public void Should_order_by_id_when_call_SelectAll()
        {
            //arrange 
            var repository = new EntityFrameworkRepository<Model>(_contextFactory);

            //act
            var items = repository.Select(
                order: p => p.OrderByDescending(o => o.Id)
            );

            //assert
            Assert.Equal(items.First().Id, 4);
        }

        [Fact(DisplayName = "[EF_REP] : Should order by id adding navigation when call SelectAll")]
        public void Should_order_by_id_adding_navigation_when_call_SelectAll()
        {
            //arrange 
            var repository = new EntityFrameworkRepository<Model>(_contextFactory);

            //act
            var items = repository.Select(
                order: p => p.OrderByDescending(o => o.Id),
                navigationProperties: _expressionHelper
                    .Add(p => p.Id)
                    .Add(p => p.Name)
                    .Evaluate()
            );

            //assert
            Assert.Equal(items.First().Id, 4);
        }

        [Fact(DisplayName = "[EF_REP] : Should get all items when call SelectAll")]
        public void Should_get_all_items_when_call_SelectAll()
        {
            //arrange 
            var repository = new EntityFrameworkRepository<Model>(_contextFactory);

            //act
            var items = repository.Select();

            //assert
            Assert.Equal(items.Count, 4);
        }
    }

    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
