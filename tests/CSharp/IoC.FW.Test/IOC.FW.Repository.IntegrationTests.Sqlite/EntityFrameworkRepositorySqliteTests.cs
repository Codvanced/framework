using IOC.FW.Abstraction.Repository;
using IOC.FW.Repository.EntityFramework;
using IOC.FW.Shared.Model.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace IOC.FW.Repository.IntegrationTests.Sqlite
{
    public class EntityFrameworkRepositorySqliteTests
    {
        private readonly IContextFactory<Model> _contextFactory;
        private readonly IRepository<Model> _repository;
        private readonly Model[] initialModelSetup;
        private readonly ModelRef[] initialModelRefSetup;
        private readonly ExpressionAppender<Model> _expressionHelper;
        private const string DATABASE_WIPE_AND_SETUP = @"
DROP TABLE IF EXISTS ModelRef;
DROP TABLE IF EXISTS Model;

CREATE TABLE Model (
    Id   INTEGER       NOT NULL PRIMARY KEY AUTOINCREMENT
,   Name VARCHAR (200) NOT NULL
);

CREATE TABLE ModelRef (
    Id      INTEGER       NOT NULL PRIMARY KEY AUTOINCREMENT
,   IdRef   INTEGER       NOT NULL 
,   Name    VARCHAR (200) NOT NULL
,   FOREIGN KEY(IdRef) REFERENCES Model(Id)
);

INSERT INTO Model(Name) VALUES('Test');
INSERT INTO Model(Name) VALUES('Test 2');
INSERT INTO Model(Name) VALUES('Test 3');
INSERT INTO Model(Name) VALUES('Test 4');
INSERT INTO Model(Name) VALUES('Test 5');

INSERT INTO ModelRef(IdRef, Name) VALUES(1, 'Ref Test');
INSERT INTO ModelRef(IdRef, Name) VALUES(1, 'Ref Test 2');
INSERT INTO ModelRef(IdRef, Name) VALUES(1, 'Ref Test 3');
INSERT INTO ModelRef(IdRef, Name) VALUES(2, 'Ref Test 4');
INSERT INTO ModelRef(IdRef, Name) VALUES(2, 'Ref Test 5');
";

        public EntityFrameworkRepositorySqliteTests()
        {
            _expressionHelper = new ExpressionAppender<Model>();
            _contextFactory = new ContextFactory<Model>();
            _repository = new EntityFrameworkRepository<Model>(_contextFactory);

            initialModelRefSetup = new ModelRef[] {
                new ModelRef { Id = 1, IdRef = 1, Name = "Test", @Model = null },
                new ModelRef { Id = 2, IdRef = 1, Name = "Test 2", @Model = null },
                new ModelRef { Id = 3, IdRef = 1, Name = "Test 3", @Model = null },
                new ModelRef { Id = 4, IdRef = 2, Name = "Test 4", @Model = null },
                new ModelRef { Id = 5, IdRef = 2, Name = "Test 5", @Model = null }
            };

            initialModelSetup = new Model[] {
                new Model { Id = 1, Name = "Test", ModelRefs = new List<ModelRef> {
                    initialModelRefSetup[0], initialModelRefSetup[1], initialModelRefSetup[2]
                } },
                new Model { Id = 2, Name = "Test 2", ModelRefs = new List<ModelRef> {
                    initialModelRefSetup[3], initialModelRefSetup[4]
                } },
                new Model { Id = 3, Name = "Test 3", ModelRefs = new List<ModelRef> {
                } },
                new Model { Id = 4, Name = "Test 4", ModelRefs = new List<ModelRef> {
                } },
                new Model { Id = 5, Name = "Test 5", ModelRefs = new List<ModelRef> {
                } }
            };

            initialModelRefSetup[0].Model = initialModelSetup[0];
            initialModelRefSetup[1].Model = initialModelSetup[0];
            initialModelRefSetup[2].Model = initialModelSetup[0];
            initialModelRefSetup[3].Model = initialModelSetup[1];
            initialModelRefSetup[4].Model = initialModelSetup[1];

            _repository.ExecuteQuery(
                sql: DATABASE_WIPE_AND_SETUP,
                parametersWithDirection: null,
                cmdType: System.Data.CommandType.Text
            );
        }

        [Fact(DisplayName = "[EF_REP_INTEGRATION] : Should select all data inserted on setup phase")]
        public void Should_select_all_data_inserted_on_setup_phase()
        {
            var allItems = _repository.Select();

            Assert.NotEmpty(allItems);
            Assert.Equal(initialModelSetup.Length, allItems.Count);
            Assert.Equal(initialModelSetup.First().Id, allItems.First().Id);
        }

        [Fact(DisplayName = "[EF_REP_INTEGRATION] : Should select all data ordered by id desc inserted on setup phase")]
        public void Should_select_all_data_ordered_by_id_desc_inserted_on_setup_phase()
        {
            var allItems = _repository.Select(
                order: o => o.OrderByDescending(p => p.Id)
            );

            Assert.NotEmpty(allItems);
            Assert.Equal(initialModelSetup.Length, allItems.Count);
            Assert.Equal(initialModelSetup.Last().Id, allItems.First().Id);
        }

        [Fact(DisplayName = "[EF_REP_INTEGRATION] : Should return all data with references when call SelectAll with navigation properties")]
        public void Should_return_all_data_with_references_when_call_SelectAll_with_navigation_properties()
        {
            var allItems = _repository.Select(
                navigationProperties: _expressionHelper.Add(
                    n => n.ModelRefs
                ).Evaluate()
            );

            Assert.NotEmpty(allItems);
            Assert.Equal(initialModelSetup.Length, allItems.Count);
            Assert.Equal(initialModelSetup.First().Id, allItems.First().Id);
            Assert.Equal(initialModelSetup.First().ModelRefs.Count, allItems.First().ModelRefs.Count);
            Assert.Equal(initialModelSetup.First().ModelRefs.First().Id, allItems.First().ModelRefs.First().Id);
        }

        [Fact(DisplayName = "[EF_REP_INTEGRATION] : Should return all data with references and ordered when call SelectAll with navigation properties and order")]
        public void Should_return_all_data_with_references_and_ordered_when_call_SelectAll_with_navigation_properties_and_order()
        {
            var allItems = _repository.Select(
                order: o => o.OrderByDescending(p => p.Id),
                navigationProperties: _expressionHelper.Add(n => n.ModelRefs).Evaluate()
            );

            Assert.NotEmpty(allItems);
            Assert.Equal(initialModelSetup.Length, allItems.Count);
            Assert.Equal(initialModelSetup.Last().Id, allItems.First().Id);
            Assert.Equal(0, allItems.First().ModelRefs.Count);
        }

        [Fact(DisplayName = "[EF_REP_INTEGRATION] : Should return all data filtered when call Select with where clause")]
        public void Should_return_all_data_filtered_when_call_Select_with_where_clause()
        {
            var allItems = _repository.Select(where: w => w.Name.Contains("Test "));

            Assert.NotEmpty(allItems);
            Assert.Equal(initialModelRefSetup.Length - 1, allItems.Count);
        }

        [Fact(DisplayName = "[EF_REP_INTEGRATION] : Should return all data with references and filtered when call Select with where clause and navigation properties")]
        public void Should_return_all_data_with_references_and_filtered_when_call_Select_with_where_clause_and_navigation_properties()
        {
            var allItems = _repository.Select(
                where: w => w.Name.Contains("Test "),
                navigationProperties: _expressionHelper.Add(n => n.ModelRefs).Evaluate()
            );

            Assert.NotEmpty(allItems);
            Assert.Equal(initialModelRefSetup.Length - 1, allItems.Count);
            Assert.Equal(allItems.First().ModelRefs.Count, 2);
            Assert.Equal(allItems.First().Id, initialModelSetup[1].Id);
            Assert.Equal(allItems.First().ModelRefs.First().Id, initialModelSetup[1].ModelRefs.First().Id);
        }

        [Fact(DisplayName = "[EF_REP_INTEGRATION] : Should return all data with references and ordered when call Select with where clause and order")]
        public void Should_return_all_data_with_references_and_ordered_when_call_Select_with_where_clause_and_order()
        {
            var allItems = _repository.Select(
                where: w => w.Name.Contains("Test "),
                order: o => o.OrderBy(p => p.Id)
            );

            Assert.NotEmpty(allItems);
            Assert.Equal(4, allItems.Count);
            Assert.Equal(initialModelSetup[1].Id, allItems.First().Id);
        }

        [Fact(DisplayName = "[EF_REP_INTEGRATION] : Should return all data with references ordered and filtered when call Select with where clause navigation properties and order")]
        public void Should_return_all_data_with_references_ordered_and_filtered_when_call_Select_with_where_clause_navigation_properties_and_order()
        {
            var allItems = _repository.Select(
                where: w => w.Name.Contains("Test "),
                order: o => o.OrderBy(p => p.Id),
                navigationProperties: _expressionHelper.Add(n => n.ModelRefs).Evaluate()
            );

            Assert.NotEmpty(allItems);
            Assert.Equal(4, allItems.Count);
            Assert.Equal(allItems.First().ModelRefs.Count, 2);
            Assert.Equal(allItems.First().Id, initialModelSetup[1].Id);
            Assert.Equal(allItems.First().ModelRefs.First().Id, initialModelSetup[1].ModelRefs.First().Id);
        }

        [Fact(DisplayName = "[EF_REP_INTEGRATION] : Should return only one data ordered when call SelectSingle with order")]
        public void Should_return_only_one_data_ordered_when_call_SelectSingle_with_order()
        {
            var item = _repository.SelectSingle(order: o => o.OrderByDescending(p => p.Id));

            Assert.NotNull(item);
            Assert.Equal(item.Id, 5);
        }

        [Fact(DisplayName = "[EF_REP_INTEGRATION] : Should return only one data filtered and with references when call SelectSingle with where clause and order")]
        public void Should_return_only_one_data_filtered_and_with_references_when_call_SelectSingle_with_where_clause_and_order()
        {
            var item = _repository.SelectSingle(
                where: w => w.Name.Contains("Test "),
                navigationProperties: _expressionHelper.Add(n => n.ModelRefs).Evaluate()
            );

            Assert.NotNull(item);
            Assert.Equal(2, item.Id);
        }

        [Fact(DisplayName = "[EF_REP_INTEGRATION] : Should return only one data filtered when call SelectSingle with where clause")]
        public void Should_return_only_one_data_filtered_when_call_SelectSingle_with_where_clause()
        {
            var item = _repository.SelectSingle(
                where: w => w.Name.Contains("Test 2")
            );

            Assert.NotNull(item);
            Assert.Equal(2, item.Id);
        }

        [Fact(DisplayName = "[EF_REP_INTEGRATION] : Should return only one data ordered when call SelectSingle with where clause and order")]
        public void Should_return_only_one_data_ordered_when_call_SelectSingle_with_where_clause_and_order()
        {
            var item = _repository.SelectSingle(
                where: w => w.Name.Contains("Test "),
                order: o => o.OrderByDescending(p => p.Id)
            );

            Assert.NotNull(item);
            Assert.Equal(item.Id, 5);
        }

        [Fact(DisplayName = "[EF_REP_INTEGRATION] : Should return only one data with references and filtered when call SelectSingle with order and navigation properties")]
        public void Should_return_only_one_data_with_references_and_filtered_when_call_SelectSingle_with_order_and_navigation_properties()
        {
            var item = _repository.SelectSingle(
                order: o => o.OrderByDescending(p => p.Id),
                navigationProperties: _expressionHelper.Add(n => n.ModelRefs).Evaluate()
            );

            Assert.NotNull(item);
            Assert.Equal(item.Id, 5);
        }

        [Fact(DisplayName = "[EF_REP_INTEGRATION] : Should return only one data with references filtered and ordered when call SelectSingle with order navigation properties and where clause")]
        public void Should_return_only_one_data_with_references_filtered_and_ordered_when_call_SelectSingle_with_order_navigation_properties_and_where_clause()
        {
            var item = _repository.SelectSingle(
                where: w => w.Name.Contains("Test "),
                order: o => o.OrderByDescending(p => p.Id),
                navigationProperties: _expressionHelper.Add(n => n.ModelRefs).Evaluate()
            );

            Assert.NotNull(item);
            Assert.Equal(item.Id, 5);
        }


        [Fact(DisplayName = "[EF_REP_INTEGRATION] : Should return the max value when call Max with where clause")]
        public void Should_return_the_max_value_when_call_Max_with_where_clause()
        {
            Expression<Func<Model, bool>> whereClause = (w => w.Name.Contains("Test "));
            Func<Model, int> maxSelector = s => s.Id;

            var item = _repository.Max(
                where: whereClause,
                maxSelector: maxSelector
            );

            Assert.NotNull(item);
            Assert.Equal(
                initialModelSetup
                    .AsQueryable()
                    .Where(whereClause)
                    .Max(maxSelector),
                item
            );
        }

        [Fact(DisplayName = "[EF_REP_INTEGRATION] : Should return the min value when call Max with where clause")]
        public void Should_return_the_min_value_when_call_Min_with_where_clause()
        {
            Expression<Func<Model, bool>> whereClause = (w => w.Name.Contains("Test "));
            Func<Model, int> minSelector = s => s.Id;

            var item = _repository.Min(
                where: whereClause,
                minSelector: minSelector
            );

            Assert.NotNull(item);
            Assert.Equal(
                initialModelSetup
                    .AsQueryable()
                    .Where(whereClause)
                    .Min(minSelector), 
                item
            );
        }

    }

    [Table("Model")]
    public class Model
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<ModelRef> ModelRefs { get; set; }
    }

    [Table("ModelRef")]
    public class ModelRef
    {
        [Key]
        public int Id { get; set; }


        public int IdRef { get; set; }

        [ForeignKey("IdRef")]
        public Model Model { get; set; }

        public string Name { get; set; }
    }
}
