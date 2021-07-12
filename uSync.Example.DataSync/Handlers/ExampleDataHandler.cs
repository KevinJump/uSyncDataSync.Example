using System;
using System.Collections.Generic;
using System.Linq;

using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Logging;

using uSync8.BackOffice;
using uSync8.BackOffice.Configuration;
using uSync8.BackOffice.Services;
using uSync8.BackOffice.SyncHandlers;
using uSync8.Core;
using uSync8.Core.Serialization;

using uSyncExample.Data.Models;
using uSyncExample.Data.Services;

using static Umbraco.Core.Constants;

namespace uSync.Example.DataSync.Handlers 
{ 

    /// <summary>
    ///  Handlers manage the reading and writing of xml from disk, passing things to the serializers and saving 
    ///  items back (via your own services). 
    /// </summary>
    /// <remarks>
    ///  the syncHandler attribute defines most of how a handler looks to the user, SyncHandlerRoot class does
    ///  most of the work.
    /// </remarks>
    [SyncHandler("exampleDataHandler", "Example data", "ExampleData", uSyncBackOfficeConstants.Priorites.USYNC_RESERVED_UPPER + 100,
    Icon = "icon-lab color-orange")]
    public class ExampleDataHandler : SyncHandlerRoot<ExampleDataModel, ExampleDataModel>
        , ISyncExtendedHandler, ISyncItemHandler
    {
        // public override string Group => uSyncBackOfficeConstants.Groups.Settings;
        
        public override string Group => "Example";


        private ExampleDataService dataService;

        public ExampleDataHandler(
            ExampleDataService exampleDataService,
            IProfilingLogger logger, 
            AppCaches appCaches, 
            ISyncSerializer<ExampleDataModel> serializer, 
            ISyncItemFactory itemFactory, 
            SyncFileService syncFileService) : base(logger, appCaches, serializer, itemFactory, syncFileService)
        {
            this.dataService = exampleDataService;
        }

        // As in the serializer - there are some helper functions that, tell a handler how to 
        // find and manage your objects. the HandlerRoot service is doing all of the work
        // and providing these helper functions is all you need to do. 

        protected override IEnumerable<uSyncAction> DeleteMissingItems(ExampleDataModel parent, IEnumerable<Guid> keysToKeep, bool reportOnly)
            => Enumerable.Empty<uSyncAction>();

        protected override void DeleteViaService(ExampleDataModel item)
            => dataService.Delete(item);


        /// <summary>
        ///  GetChildItems will be called by Export - for the root parent will be null. 
        /// </summary>
        /// <remarks>
        ///  if you data is hierarchical, then you will want to also work when the parent is something. 
        ///  but for flat structures just return an empty list when the parent is set and everything
        ///  will work flat. 
        /// </remarks>
        protected override IEnumerable<ExampleDataModel> GetChildItems(ExampleDataModel parent)
        {
            if (parent == null)
                return dataService.GetAll();

            return Enumerable.Empty<ExampleDataModel>();
        }

        /// <summary>
        ///  if your data is stored in something that isn't the same type of object (eg. a folder object)
        ///  you need to impliment a slightly diffrent method to get the children.
        /// </summary>
        /// <remarks>
        ///  In this example we have assumed everything is ExampleDataModel. this method exsits 
        ///  because inside Umbraco things like DataTypes actually live in DataTypeContainer objects
        ///  so we need to cater for this.
        /// </remarks>
        protected override IEnumerable<ExampleDataModel> GetFolders(ExampleDataModel parent)
            => Enumerable.Empty<ExampleDataModel>();

        protected override ExampleDataModel GetFromService(int id)
            => dataService.Get(id);

        protected override ExampleDataModel GetFromService(Guid key)
            => dataService.Get(key);

        protected override ExampleDataModel GetFromService(string alias)
            => dataService.Get(alias);

        protected override ExampleDataModel GetFromService(ExampleDataModel item)
            => dataService.Get(item.Id);

        protected override Guid GetItemKey(ExampleDataModel item)
            => item.Key;

        protected override string GetItemName(ExampleDataModel item)
            => item.Title;


        /// <summary>
        ///  the path to how your items are saved. 
        /// </summary>
        /// <remarks>
        ///  this method determains the folder and file name used to save an object. 
        ///  
        ///  most of the time this is the alias or a guid - depending on teh settings 
        ///  used in uSync . if you want to store your data in a folder tree you would
        ///  need to return the folder paths here.
        /// </remarks>
        protected override string GetItemPath(ExampleDataModel item, bool useGuid, bool isFlat)
            => useGuid ? item.Key.ToString() : item.Alias.ToSafeFileName();

        protected override void InitializeEvents(HandlerSettings settings)
        {
            // if you service fires events when people save or delete you would add
            // listeners here. 
        }
    }
}
