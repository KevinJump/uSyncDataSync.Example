
using Umbraco.Core.Migrations;

using uSyncExample.Data.Persistance.Models;

namespace uSyncExample.Data.Persistance.Migrations
{
    public class CreateTablesMigration : MigrationBase
    {
        public CreateTablesMigration(IMigrationContext context) : base(context)
        { }

        public override void Migrate()
        {
            if (!TableExists(uSyncExampleData.TableName))
                Create.Table<ExampleDataModelDTO>().Do();
        }
    }
}
