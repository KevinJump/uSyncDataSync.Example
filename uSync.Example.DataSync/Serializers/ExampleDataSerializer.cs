using System;
using System.Xml.Linq;

using Umbraco.Core.Logging;

using uSync8.Core;
using uSync8.Core.Extensions;
using uSync8.Core.Models;
using uSync8.Core.Serialization;

using uSyncExample.Data.Models;
using uSyncExample.Data.Services;

namespace uSync.Example.DataSync.Serializers
{
    /// <summary>
    ///  in uSync Serializers are responsible for reading/writing items to and from XML 
    /// </summary>
    /// <remarks>
    ///  the exact format doesn't matter to uSync, but you can use all the existing helper methods
    ///  if you stick to the standard format of having a key and alias attribute in the root xml element.
    /// </remarks>
    [SyncSerializer("B44609DE-0577-42CF-BF82-D63AE8D5805E", "Example Data Serializer",
        "ExampleData", IsTwoPass = false)]
    public class ExampleDataSerializer : SyncSerializerRoot<ExampleDataModel>, ISyncNodeSerializer<ExampleDataModel>
    {
        private readonly ExampleDataService dataService;

        public ExampleDataSerializer(ILogger logger,
            ExampleDataService exampleDataService)
            : base(logger)
        {
            dataService = exampleDataService;
        }

        /// <summary>
        ///  Serialize the item to xml
        /// </summary>
        protected override SyncAttempt<XElement> SerializeCore(ExampleDataModel item, SyncSerializerOptions options)
        {
            var node = InitializeBaseNode(item, item.Alias);

            // basic example we serialize the model to the root of the xml
            node.Add(new XElement(nameof(ExampleDataModel.Title), item.Title));
            node.Add(new XElement(nameof(ExampleDataModel.Description), item.Description));
            node.Add(new XElement(nameof(ExampleDataModel.Value), item.Value));
            node.Add(new XElement(nameof(ExampleDataModel.DocTypeKey), item.DocTypeKey.ToString() ?? Guid.Empty.ToString()));

            // NOTE: we don't serialize id or created date,
            //
            // you do not want to serialize internal database id values, you should always have something
            // you can identify an item with that isn't the internal id (e.g Guid Key or unique alias).

            return SyncAttempt<XElement>.Succeed(item.Title, node, ChangeType.Export);
        }


        /// <summary>
        ///  Deserialize an element from xml back into an POC item
        /// </summary>
        protected override SyncAttempt<ExampleDataModel> DeserializeCore(XElement node, SyncSerializerOptions options)
        {
            // Find item method in base class will look based on key and alias for an existing item
            // if a match is found it will return the item - else null.
            var item = FindItem(node);
            if (item == null)
            {
                // item doesn't exist - create it
                item = new ExampleDataModel();
            }

            // take the XML and serialize it back into the item object. 
            item.Key = node.GetKey();
            item.Alias = node.GetAlias();
            item.Title = node.Element(nameof(ExampleDataModel.Title)).ValueOrDefault(string.Empty);
            item.Description = node.Element(nameof(ExampleDataModel.Description)).ValueOrDefault(string.Empty);
            item.Value = node.Element(nameof(ExampleDataModel.Value)).ValueOrDefault(0);
            item.DocTypeKey = node.Element(nameof(ExampleDataModel.DocTypeKey)).ValueOrDefault(Guid.Empty);

            // return success 
            return SyncAttempt<ExampleDataModel>.Succeed(item.Title, item, ChangeType.Import);

            // NOTE: we do not save it to the DB here. the handler will save the item
            // (or all items - depending on the settings) when the import completes.
        }


        // ///////// Item access / setup

        // These methods are called within the base class of the serializer 
        // and tell the core methods how to save, delete and idenitify your 
        // items. 


        /// <summary>
        ///  delete an item using our service
        /// </summary>
        /// <param name="item"></param>
        protected override void DeleteItem(ExampleDataModel item)
            => dataService.Delete(item);

        /// <summary>
        ///  save an item using our service
        /// </summary>
        protected override void SaveItem(ExampleDataModel item)
            => dataService.Save(item);

        // if you item doesn't have an unique key or alias, you can return null for
        // these methods - but at least one of FindItem(Guid) or FindItem(string) 
        // must return something for the item to be findable by usync.

        protected override ExampleDataModel FindItem(Guid key)
            => dataService.Get(key);

        protected override ExampleDataModel FindItem(string alias)
            => dataService.Get(alias);


        /// <summary>
        ///  unique alias for an item. should aways return and is what will be used 
        ///  when searching by alias
        /// </summary>
        protected override string ItemAlias(ExampleDataModel item)
            => item.Alias;

        /// <summary>
        ///  unique key for an item
        /// </summary>
        protected override Guid ItemKey(ExampleDataModel item)
            => item.Key;

    }
}
