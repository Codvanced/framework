using IOC.FW.Abstraction.Repository;
using IOC.FW.Repository.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Xunit;

namespace IOC.FW.Repository.IntegrationTests.Sqlite
{
    public class EntityFrameworkRepositorySqliteTests
    {
        private readonly IContextFactory<Model> _contextFactory;
        private readonly IRepository<Model> _repository;
        private readonly Model[] initialSetup;
        private const string DATABASE_WIPE_AND_SETUP = @"
DROP TABLE IF EXISTS Model;

CREATE TABLE Model (
    Id   INTEGER       NOT NULL PRIMARY KEY AUTOINCREMENT,
    Name VARCHAR (200) NOT NULL
);

INSERT INTO Model(Name) VALUES('Test');
INSERT INTO Model(Name) VALUES('Test 2');
INSERT INTO Model(Name) VALUES('Test 3');
INSERT INTO Model(Name) VALUES('Test 4');
INSERT INTO Model(Name) VALUES('Test 5');";

        public EntityFrameworkRepositorySqliteTests()
        {
            _contextFactory = new ContextFactory<Model>();
            _repository = new EntityFrameworkRepository<Model>(_contextFactory);
            initialSetup = new Model[] {
                new Model { Id = 1, Name = "Test" },
                new Model { Id = 2, Name = "Test 2" },
                new Model { Id = 3, Name = "Test 3" },
                new Model { Id = 4, Name = "Test 4" },
                new Model { Id = 5, Name = "Test 5" }
            };

            _repository.ExecuteQuery(
                sql: DATABASE_WIPE_AND_SETUP,
                parametersWithDirection: null,
                cmdType: System.Data.CommandType.Text
            );
        }

        [Fact(DisplayName = "[EF_REP_INTEGRATION] : Should select all data inserted on setup phase")]
        public void Should_select_all_data_inserted_on_setup_phase()
        {
            var allItems = _repository.SelectAll();

            Assert.Equal(allItems.Count, 5);
            Assert.Equal(allItems.First().Id, initialSetup.First().Id);
        }

        [Fact(DisplayName = "[EF_REP_INTEGRATION] : Should select all data ordered by id desc inserted on setup phase")]
        public void Should_select_all_data_ordered_by_id_desc_inserted_on_setup_phase()
        {
            var allItems = _repository.SelectAll(
                order: o => o.OrderByDescending(p => p.Id)
            );

            Assert.Equal(allItems.Count, 5);
            Assert.Equal(allItems.First().Id, initialSetup.Last().Id);
        }

    }

    [Table("Model")]
    public class Model
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
