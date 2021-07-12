
using Umbraco.Core.Migrations;

using uSyncExample.Data.Persistance.Migrations;

namespace uSyncExample.Data.Persistance
{
    public class ExampleDataMigrationPlan : MigrationPlan
    {
        public ExampleDataMigrationPlan()
            : base("uSyncExampleData")
        {
            From(string.Empty)
                .To<CreateTablesMigration>("Create-Tables")
                .To<AddDummyDataMigration>("Add-Dummy-Data");
        }
    }
}
