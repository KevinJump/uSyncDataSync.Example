using System.Collections.Generic;

using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

using uSyncExample.Data.Models;
using uSyncExample.Data.Services;

namespace uSyncExample.Data.Controller
{
    [PluginController("ExampleData")]
    public class ExampleDataApiController : UmbracoAuthorizedApiController
    {
        private readonly ExampleDataService _exampleDataService;

        public ExampleDataApiController(ExampleDataService exampleDataService)
        {
            _exampleDataService = exampleDataService;
        }

        public bool GetApi() => true;

        public IEnumerable<ExampleDataModel> GetAll()
            => _exampleDataService.GetAll();

        public ExampleDataModel Get(int id)
            => _exampleDataService.Get(id);

    }
}
