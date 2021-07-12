using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using uSyncExample.Data.Models;
using uSyncExample.Data.Persistance.Models;

namespace uSyncExample.Data.Persistance
{
    public static class ExampleDataMapping
    {

        public static ExampleDataModel ToModel(this ExampleDataModelDTO dto)
        {
            return new ExampleDataModel
            {
                Id = dto.Id,
                Key = dto.Key,
                Created = dto.Created,
                Alias = dto.Alias,
                Title = dto.Title,
                Description = dto.Description,
                DocTypeKey = dto.DocTypeKey,
                Value = dto.Value
            };
        }

        public static ExampleDataModelDTO ToDTO(this ExampleDataModel model)
        {
            return new ExampleDataModelDTO
            {
                Id = model.Id,
                Key = model.Key,
                Created = model.Created,
                Alias = model.Alias,
                Title = model.Title,
                Description = model.Description,
                DocTypeKey = model.DocTypeKey,
                Value = model.Value
            };
        }

    }
}
