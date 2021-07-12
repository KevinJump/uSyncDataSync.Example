
using Umbraco.Core;
using Umbraco.Core.Composing;

using uSync.Example.DataSync.Serializers;

using uSync8.BackOffice;
using uSync8.Core;
using uSync8.Core.Serialization;

using uSyncExample.Data;
using uSyncExample.Data.Models;

namespace uSync.Example.DataSync
{
    /// <summary>
    ///  Composer to register serializers. 
    /// </summary>
    /// <remarks>
    ///  Serializers have to be:
    ///  
    ///     - registered before uSyncBackOffice loads the handlers
    ///     - registered after the uSyncCore has loaded the core things (like mappers)
    ///     
    ///     - in this example also registered after the example data repo/service have been setup
    ///  
    /// </remarks>
    [ComposeAfter(typeof(ExampleDataComposer))]
    [ComposeAfter(typeof(uSyncCoreComposer))]
    [ComposeBefore(typeof(uSyncBackOfficeComposer))]
    public class ExampleDataSyncComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            // only the serializer has to be registered 
            composition.Register<ISyncSerializer<ExampleDataModel>, ExampleDataSerializer>();

            // handlers, mappers etc are loaded by type 
        }
    }
}
