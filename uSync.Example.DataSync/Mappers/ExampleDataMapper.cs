using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

using uSync8.ContentEdition.Mapping;
using uSync8.Core.Dependency;

using uSyncExample.Data.Models;
using uSyncExample.Data.Services;

using static Umbraco.Core.Constants;

namespace uSync.Example.DataSync.Mappers
{
    /// <summary>
    ///  Mappers are used by uSync.ContentEdition to apply changes to data as it is imported and 
    ///  exported as part of a content or media item. it also calculates dependencies when using 
    ///  uSync.Complete
    /// </summary>
    /// <remarks>
    ///  mostly you don't need to worry about Export or Import values unless you are storing DB 
    ///  specific values within the property value. 
    ///  
    ///  for the sake of this example we have shown how you might remove and reinsert the DB id 
    ///  value for a property as it is exported and imported, but if you store a GUID or alias
    ///  value that is portable you don't need to override the GetExportValue or GetImportValue
    ///  methods.
    ///  / 
    /// </remarks>
    public class ExampleDataMapper : SyncValueMapperBase, ISyncMapper
    {
        private readonly ExampleDataService dataService;

        public ExampleDataMapper(IEntityService entityService,
            ExampleDataService exampleDataService) :
            base(entityService)
        {
            this.dataService = exampleDataService;
        }

        public override string Name => "Example data value mapper";

        public override string[] Editors => new string[]
        {
            "exampleDataProperty"
        };

        public override string GetExportValue(object value, string editorAlias)
        {
            var stringValue = GetValueAs<string>(value);
            if (stringValue != null && stringValue.DetectIsJson())
            {
                var exampleData = JsonConvert.DeserializeObject<JObject>(stringValue);
                exampleData.Remove("id");

                return JsonConvert.SerializeObject(exampleData);
            }

            return value.ToString();
        }

        public override string GetImportValue(string value, string editorAlias)
        {
            var item = GetDataModel(value);
            if (item != null)
                return JsonConvert.SerializeObject(item);

            return value;
        }

        /// <summary>
        ///  return any dependencies that are required for this property.
        /// </summary>
        /// <remarks>
        ///  when your property requires other things to be present to work, we need
        ///  to tell uSync.Complete about that dependency.
        ///  
        ///  in this example we will tell it about our own Item object and a linked
        ///  document type.
        ///  
        ///  if we have our own custom objects we need to have a 'ServiceConnector' class 
        ///  somewhere because uSync needs to know the UDI for the item. so it can stream 
        ///  it between installations. 
        /// 
        /// </remarks>
        public override IEnumerable<uSyncDependency> GetDependencies(object value, string editorAlias, DependencyFlags flags)
        {

            var item = GetDataModel(value);
            if (item != null)
            {
                var dependencies = new List<uSyncDependency>();

                // add a dependency for this item, the UDI (containing the Key Value) 
                // is how we are telling uSync the unique id of this item, 
                // when this gets to the other site, it will be used to find the item with 
                // our custom handler and serializer.
                dependencies.Add(new uSyncDependency()
                {
                    // flags tell uSync how this dependency was calculated
                    // 99.9% of the time the flags doesn't matter so we just pass them through.
                    Flags = flags, 
                    Level = 0,
                    Name = item.Title,
                    Udi = Udi.Create(uSyncExampleDataSync.EntityType, item.Key)
                });

                // add the DocType that we might reference from our object. 
                // you only need to return the direct dependnecy (e.g the doctype)
                // and not all the grandchild ones (e.g the datatypes) they 
                // will be calculated by usync before sending.

                var docTypeDependency = GetDocTypeDependency(item);
                if (docTypeDependency != null)
                    dependencies.Add(docTypeDependency);

                return dependencies;
            }

            return Enumerable.Empty<uSyncDependency>();
        }

        /// <summary>
        ///  get a doctype dependency if there is one in our model.
        /// </summary>
        private uSyncDependency GetDocTypeDependency(ExampleDataModel model)
        {
            if (model.DocTypeKey != Guid.Empty)
            {
                var docType = entityService.Get(model.DocTypeKey, UmbracoObjectTypes.DocumentType);
                if (docType != null)
                {
                    var docTypeUdi = Udi.Create(UdiEntityType.DocumentType, docType.Key);
                    return CreateDependency(docTypeUdi.ToString(), DependencyFlags.None);
                }
            }

            return null;
        }


        /// <summary>
        ///  helper method to turn the string/object value we get passed into 
        ///  our ExampleDataModel.
        /// </summary>
        private ExampleDataModel GetDataModel(object value)
        {
            var stringValue = GetValueAs<string>(value);
            if (stringValue != null && stringValue.DetectIsJson())
            {
                var exampleData = JsonConvert.DeserializeObject<JObject>(stringValue);
                var keyAttempt = exampleData["key"].TryConvertTo<Guid>();
                if (keyAttempt.Success)
                    return dataService.Get(keyAttempt.Result);
            }

            return null;
                    

        }
    }
}
