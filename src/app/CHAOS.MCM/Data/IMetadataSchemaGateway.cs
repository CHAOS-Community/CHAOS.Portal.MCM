using System;
using System.Collections.Generic;
using Chaos.Mcm.Data.Dto;
using Chaos.Mcm.Permission;

namespace Chaos.Mcm.Data
{
  public interface IMetadataSchemaGateway
  {
    IList<MetadataSchema> MetadataSchemaGet(Guid userGuid, IEnumerable<Guid> groupGuids, Guid? metadataSchemaGuid, MetadataSchemaPermission permission);
    IList<MetadataSchema> MetadataSchemaGet(Guid? metadataSchemaGuid = null);
    uint MetadataSchemaUpdate(string name, string schema, Guid userGuid, Guid guid);
    uint MetadataSchemaUpdate(MetadataSchema schema);
    uint MetadataSchemaCreate(string name, string schema, Guid userGuid, Guid guid);
    uint MetadataSchemaCreate(MetadataSchema schema, Guid userGuid);
    uint MetadataSchemaDelete(Guid guid);
  }
}