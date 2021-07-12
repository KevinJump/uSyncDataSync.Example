using System;
using System.Collections.Generic;

using Umbraco.Core.Logging;
using Umbraco.Core.Scoping;

using uSyncExample.Data.Models;
using uSyncExample.Data.Persistance;

namespace uSyncExample.Data.Services
{
    public class ExampleDataService
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly ExampleDataRepository _dataRespository;
        private readonly ILogger _logger;

        public ExampleDataService(
            IScopeProvider scopeProvider,
            ExampleDataRepository dataRepository,
            ILogger logger)
        {
            _scopeProvider = scopeProvider;
            _dataRespository = dataRepository;
            _logger = logger;
        }


        public ExampleDataModel Get(int id)
        {
            using (_scopeProvider.CreateScope(autoComplete: true))
            {
                return _dataRespository.Get(id);
            }
        }

        public ExampleDataModel Get(Guid key)
        {
            using (_scopeProvider.CreateScope(autoComplete: true))
            {
                return _dataRespository.Get(key);
            }
        }

        public ExampleDataModel Get(string alias)
        {
            using (_scopeProvider.CreateScope(autoComplete: true))
            {
                return _dataRespository.Get(alias);
            }
        }

        public IEnumerable<ExampleDataModel> GetAll(params int[] ids)
        {
            using (_scopeProvider.CreateScope(autoComplete: true))
            {
                return _dataRespository.GetAll(ids);
            }
        }

        public ExampleDataModel Save(ExampleDataModel model)
        {
            using (_scopeProvider.CreateScope(autoComplete: true))
            {
                return _dataRespository.Save(model);
            }
        }

        public void Delete(ExampleDataModel model)
        {
            using (_scopeProvider.CreateScope(autoComplete: true))
            {
                _dataRespository.Delete(model);
            }
        }

        public void Delete(int id)
        {
            using (_scopeProvider.CreateScope(autoComplete: true))
            {
                _dataRespository.Delete(id);
            }
        }

        public void Delete(Guid key)
        {
            using(_scopeProvider.CreateScope(autoComplete: true))
            {
                var item = _dataRespository.Get(key);
                if (item != null)
                    _dataRespository.Delete(item);
            }
        }

        public int Count()
        {
            using(_scopeProvider.CreateScope(autoComplete: true))
            {
                return _dataRespository.Count();
            }
        }

    }
}
