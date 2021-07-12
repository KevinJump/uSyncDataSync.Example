using System;

using Umbraco.Core.Migrations;

using uSyncExample.Data.Persistance.Models;

namespace uSyncExample.Data.Persistance.Migrations
{
    public class AddDummyDataMigration : MigrationBase
    {
        public AddDummyDataMigration(IMigrationContext context) : base(context)
        { }

        public override void Migrate()
        {
            if (TableExists(uSyncExampleData.TableName))
            {
                this.Database.Insert(new ExampleDataModelDTO() { Key = Guid.NewGuid(), Alias = "One", Title = "One", Description = "First One", Value = 1, Created = DateTime.Now });
                this.Database.Insert(new ExampleDataModelDTO() { Key = Guid.NewGuid(), Alias = "Two", Title = "Two", Description = "Second One", Value = 2, Created = DateTime.Now });
                this.Database.Insert(new ExampleDataModelDTO() { Key = Guid.NewGuid(), Alias = "Three", Title = "Three", Description = "Third One", Value = 3, Created = DateTime.Now });
                this.Database.Insert(new ExampleDataModelDTO() { Key = Guid.NewGuid(), Alias = "Four", Title = "Four", Description = "Fouth One", Value = 4, Created = DateTime.Now });
                this.Database.Insert(new ExampleDataModelDTO() { Key = Guid.NewGuid(), Alias = "Five", Title = "Five", Description = "Fifth One", Value = 5, Created = DateTime.Now });
            }
        }
    }
}
