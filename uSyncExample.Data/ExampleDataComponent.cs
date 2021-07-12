
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;

using uSyncExample.Data.Persistance;
using uSyncExample.Data.Services;

namespace uSyncExample.Data
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class ExampleDataComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.RegisterUnique<ExampleDataRepository>();
            composition.RegisterUnique<ExampleDataService>();

            composition.Components().Append<ExampleDataComponent>();
        }
    }

    public class ExampleDataComponent : IComponent
    {
        private readonly IScopeProvider scopeProvider;
        private readonly IMigrationBuilder migrationBuilder;
        private readonly IKeyValueService keyValueService;
        private readonly IProfilingLogger logger;

        public ExampleDataComponent(
            IScopeProvider scopeProvider,
            IMigrationBuilder migrationBuilder,
            IKeyValueService keyValueService,
            IProfilingLogger logger)
        {
            this.scopeProvider = scopeProvider;
            this.migrationBuilder = migrationBuilder;
            this.keyValueService = keyValueService;
            this.logger = logger;
        }

        public void Initialize()
        {
            var upgrader = new Upgrader(new ExampleDataMigrationPlan());
            upgrader.Execute(scopeProvider, migrationBuilder, keyValueService, logger);

        }

        public void Terminate()
        {
        }
    }
}
