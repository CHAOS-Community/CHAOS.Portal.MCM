using System;
using System.Collections.Generic;
using Chaos.Mcm.Data.Dto;
using Chaos.Mcm.Data.Dto.Standard;
using Chaos.Mcm.Permission;
using Object = Chaos.Mcm.Data.Dto.Standard.Object;

namespace Chaos.Mcm.Data
{
    using System.Xml.Linq;

    public interface IMcmRepository
    {
        IMcmRepository WithConfiguration(string connectionString);

        IEnumerable<FolderUserJoin> GetFolderUserJoin();
        uint SetFolderUserJoin(Guid userGuid, uint folderID, uint permission);
        IEnumerable<FolderGroupJoin> GetFolderGroupJoin();
        uint SetFolderGroupJoin(Guid groupGuid, uint folderID, uint permission);
        IEnumerable<Folder> GetFolder();
        int DeleteFolder(uint id);
        uint CreateFolder(Guid userGuid, Guid? subscriptionGuid, string title, uint? parentID, uint folderTypeID);
        IEnumerable<IFolderInfo> GetFolderInfo(IEnumerable<uint> ids);
        IEnumerable<AccessPoint> GetAccessPoint(Guid accessPointGuid, Guid userGuid, IEnumerable<Guid> groupGuids, uint permission);
        uint SetAccessPointPublishSettings(Guid accessPointGuid, Guid objectGuid, DateTime? startDate, DateTime? endDate);
        uint UpdateFolder(uint id, string newTitle, uint? newParentID, uint? newFolderTypeID);
        IEnumerable<Object> GetObject(IEnumerable<Guid> objectGuids, bool includeMetadata, bool includeFiles, bool includeObjectRelations, bool includeFolders, bool includeAccessPoint);
        IEnumerable<Object> GetObject(Guid objectGuid, bool includeMetadata, bool includeFiles, bool includeObjectRelations, bool includeFolders, bool includeAccessPoint);
        IEnumerable<Object> GetObject(Guid relatedToObjectWithGuid, uint? objectRelationTypeID);
        IEnumerable<MetadataSchema> GetMetadataSchema(Guid userGuid, IEnumerable<Guid> groupGuids, Guid? metadataSchemaGuid, MetadataSchemaPermission permission );

        uint ObjectRelationSet(Guid object1Guid, Guid object2Guid, uint objectRelationTypeID, int? sequence);
        uint ObjectRelationSet(ObjectRelationInfo objectRelationInfo, Guid editingUserGuid);

        IEnumerable<NewMetadata> MetadataGet(Guid guid);

        uint MetadataSet(Guid objectGuid, Guid metadataGuid, Guid metadataSchemaGuid, string languageCode, uint revisionID, XDocument metadataXml, Guid userGuid);
    }
}
