using System;
using System.Collections.Generic;
using System.Linq;

using NPoco;

using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.Querying;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Core.Scoping;

using uSyncExample.Data.Models;
using uSyncExample.Data.Persistance.Models;

namespace uSyncExample.Data.Persistance
{
    public class ExampleDataRepository
    {
        const int maxParams = 2000; // SqlCE group limit.

        private readonly IScopeAccessor _scopeAccessor;
        private readonly ILogger _logger;

        public ExampleDataRepository(IScopeAccessor scopeAccessor,
            ILogger logger)
        {
            _scopeAccessor = scopeAccessor;
            _logger = logger;
        }

        private IScope AmbientScope
        {
            get
            {
                var scope = _scopeAccessor.AmbientScope;
                if (scope == null)
                    throw new InvalidOperationException("Cannot run with an ambient scope");
                return scope;
            }
        }

        private IUmbracoDatabase Database => AmbientScope.Database;
        private ISqlContext SqlContext => AmbientScope.SqlContext;

        private Sql<ISqlContext> Sql() => SqlContext.Sql();

        private ISqlSyntaxProvider SqlSyntax => SqlContext.SqlSyntax;

        private IQuery<T> Query<T>() => SqlContext.Query<T>();



        private Sql<ISqlContext> GetBaseQuery(bool isCount)
            => isCount
                ? Sql().SelectCount().From<ExampleDataModelDTO>()
                : Sql().Select($"{uSyncExampleData.TableName}.*").From<ExampleDataModelDTO>();

        private string GetBaseWhereClause()
            => $"{uSyncExampleData.TableName}.Id = @Id";


        public ExampleDataModel Get(int id)
        {
            var sql = GetBaseQuery(false)
                .Where(GetBaseWhereClause(), new { Id = id });

            var dto = Database.FirstOrDefault<ExampleDataModelDTO>(sql);
            if (dto == null) return default;
            return dto.ToModel();
        }

        public ExampleDataModel Get(Guid key)
        {
            var sql = GetBaseQuery(false)
                .Where<ExampleDataModelDTO>(x => x.Key == key);

            var dto = Database.FirstOrDefault<ExampleDataModelDTO>(sql);
            if (dto == null) return default;
            return dto.ToModel();
        }

        public ExampleDataModel Get(string alias)
        {
            var sql = GetBaseQuery(false)
                .Where($"{uSyncExampleData.TableName}.Alias = @alias", new { Alias = alias });

            var dto = Database.FirstOrDefault<ExampleDataModelDTO>(sql);
            if (dto == null) return default;
            return dto.ToModel();
        }

        public IEnumerable<ExampleDataModel> GetAll(params int[] ids)
        {
            if (ids.Length == 0) return DoGetAll();

            var allIds = ids.Distinct().ToArray();
            if (allIds.Length <= maxParams)
            {
                return DoGetAll(allIds);
            }

            List<ExampleDataModel> results = new List<ExampleDataModel>();
            foreach(var groupOfIds in ids.InGroupsOf(maxParams))
            {
                results.AddRange(DoGetAll(groupOfIds.ToArray()));
            }

            return results;
        }

        private IEnumerable<ExampleDataModel> DoGetAll(params int[] ids)
        {
            var sql = GetBaseQuery(false);
            if (ids.Length > 0)
            {
                sql.Where($"{uSyncExampleData.TableName}.Id in (@Ids)", new { Ids = ids });
            }
            else
            {
                sql.Where($"{uSyncExampleData.TableName}.Id > 0");
            }

            return Database.Fetch<ExampleDataModelDTO>(sql)
                .Select(x => x.ToModel());
        }

        public ExampleDataModel Save(ExampleDataModel model)
        {
            var dto = model.ToDTO();

            using(var transaction = Database.GetTransaction())
            {
                Database.Save(dto);
                transaction.Complete();
            }

            return dto.ToModel();
        }

        public void Delete(ExampleDataModel model)
        {
            var dto = model.ToDTO();
            using (var transaction = Database.GetTransaction())
            {
                Database.Delete(dto);
                transaction.Complete();
            }
        }

        public void Delete(int id)
        {
            using (var transaction = Database.GetTransaction())
            {
                Database.Delete<ExampleDataModel>(id);
                transaction.Complete();
            }
        }

        public int Count()
        {
            var sql = GetBaseQuery(true);
            return Database.ExecuteScalar<int>(sql);
        }

    }
}
