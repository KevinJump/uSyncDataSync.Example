using System;

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace uSyncExample.Data.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class ExampleDataModel
    {
        public int Id { get; set; }

        public Guid Key { get; set; }

        public string Alias { get; set; }

        public DateTime Created { get; set; }

        public string Title { get; set; }

        public int Value { get; set; }

        public string Description { get; set; }

        public Guid DocTypeKey { get; set; }
    }
}
