using System;
using System.Collections.Generic;
using CHAOS.MCM.Data.Dto.Standard;
using CHAOS.MCM.Permission;
using Object = CHAOS.MCM.Data.Dto.Standard.Object;

namespace Chaos.Mcm.Data
{
    public interface IMcmRepository
    {
        IMcmRepository WithConfiguration(string connectionString);

        IEnumerable<FolderUserJoin> GetFolderUserJoin();
        uint SetFolderUserJoin(Guid userGuid, uint folderID, uint permission);
        IEnumerable<FolderGroupJoin> GetFolderGroupJoin();
        uint SetFolderGroupJoin(Guid groupGuid, uint folderID, uint permission);
        IEnumerable<Folder> GetFolder();
        uint DeleteFolder(uint id);
        uint CreateFolder(Guid userGuid, Guid? subscriptionGuid, string title, uint? parentID, uint folderTypeID);
        IEnumerable<FolderInfo> GetFolderInfo(IEnumerable<uint> ids);
        IEnumerable<AccessPoint> GetAccessPoint(Guid accessPointGuid, Guid userGuid, IEnumerable<Guid> groupGuids, uint permission);
        uint SetAccessPointPublishSettings(Guid accessPointGuid, Guid objectGuid, DateTime? startDate, DateTime? endDate);

        IEnumerable<Object> GetObject(IEnumerable<Guid> objectGuids, bool includeMetadata, bool includeFiles, bool includeObjectRelations, bool includeFolders, bool includeAccessPoint);
        IEnumerable<Object> GetObject(Guid objectGuid, bool includeMetadata, bool includeFiles, bool includeObjectRelations, bool includeFolders, bool includeAccessPoint);
        IEnumerable<Object> GetObject(Guid relatedToObjectWithGuid, uint? objectRelationTypeID);
        IEnumerable<MetadataSchema> GetMetadataSchema(Guid userGuid, IEnumerable<Guid> groupGuids, Guid? metadataSchemaGuid, MetadataSchemaPermission permission );
    }
}
