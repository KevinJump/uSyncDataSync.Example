using System;

using NPoco;

using Umbraco.Core.Persistence.DatabaseAnnotations;
using Umbraco.Core.Persistence.DatabaseModelDefinitions;

namespace uSyncExample.Data.Persistance.Models
{
    [TableName(uSyncExampleData.TableName)]
    [PrimaryKey("Id")]
    [ExplicitColumns]
    public class ExampleDataModelDTO
    {
        [Column("Id")]
        [PrimaryKeyColumn]
        public int Id { get; set; }

        [Column("Key")]
        public Guid Key { get; set; }

        [Column("Alias")]
        public string Alias { get; set; }

        [Column("Created")]
        [Constraint(Default = SystemMethods.CurrentDateTime)]
        public DateTime Created { get; set; }

        [Column("Title")]
        public string Title { get; set; }

        [Column("Value")]
        public int Value { get; set; }

        [Column("Description")]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string Description { get; set; }


        [Column("DocTypeKey")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public Guid DocTypeKey { get; set; }
    }
}
