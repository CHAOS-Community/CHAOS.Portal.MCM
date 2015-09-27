using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Chaos.Mcm.Data.Dto;
using Chaos.Mcm.Data.Dto.Standard;
using Object = System.Object;

namespace Chaos.Mcm.Data
{
  public interface IMcmRepository : IMetadataSchemaGateway
  {
    IMcmRepository WithConfiguration(string connectionString);

    uint FolderUserJoinSet(Guid userGuid, uint folderID, uint permission);
    uint FolderGroupJoinSet(Guid groupGuid, uint folderID, uint permission);
    IList<FolderPermission> FolderPermissionGet();

    uint FolderCreate(Guid userGuid, Guid? subscriptionGuid, string name, uint? parentID, uint folderTypeID);
    int FolderDelete(uint id);
    IList<Folder> FolderGet(uint? id = null, Guid? userGuid = null, Guid? objectGuid = null);
    IList<FolderInfo> FolderInfoGet(IEnumerable<uint> ids);
        
    IList<AccessPoint> AccessPointGet(Guid accessPointGuid, Guid userGuid, IEnumerable<Guid> groupGuids, uint permission);
    uint AccessPointPublishSettingsSet(Guid accessPointGuid, Guid objectGuid, DateTime? startDate, DateTime? endDate);
    uint FolderUpdate(uint id, string newName, uint? newParentID, uint? newFolderTypeID);

    uint ObjectDelete(Guid guid);
    uint ObjectCreate(Guid guid, uint objectTypeID, uint folderID);

    /// <summary>
    /// Get Object by objectGuid
    /// </summary>
    /// <param name="objectGuid"></param>
    /// <param name="includeMetadata"></param>
    /// <param name="includeFiles"></param>
    /// <param name="includeObjectRelations"></param>
    /// <param name="includeFolders"></param>
    /// <param name="includeAccessPoints"></param>
    /// <returns>The matching Object, if there is no Object by that objectGuid, null is returned</returns>
    Dto.Object ObjectGet(Guid objectGuid, bool includeMetadata = false, bool includeFiles = false, bool includeObjectRelations = false, bool includeFolders = false, bool includeAccessPoints = false);
    IList<Dto.Object> ObjectGet( IEnumerable<Guid> objectGuids, bool includeMetadata = false, bool includeFiles = false, bool includeObjectRelations = false, bool includeFolders = false, bool includeAccessPoints = false );
    IList<Dto.Object> ObjectGet(uint? folderID = null, uint pageIndex = 0, uint pageSize = 5, bool includeMetadata = false, bool includeFiles = false, bool includeObjectRelations = false, bool includeFolders = false, bool includeAccessPoints = false, uint? objectTypeId = null);

    uint ObjectRelationDelete(Guid object1Guid, Guid object2Guid, uint objectRelationTypeID);
    uint ObjectRelationSet(Guid object1Guid, Guid object2Guid, uint objectRelationTypeID, int? sequence);
    uint ObjectRelationSet( Guid object1Guid, Guid object2Guid, uint objectRelationTypeID, int? sequence, Guid metadataGuid, Guid metadataSchemaGuid, string languageCode, XDocument metadataXml, Guid editingUserGuid );

    IEnumerable<Metadata> MetadataGet(Guid guid);

    uint MetadataSet(Guid objectGuid, Guid metadataGuid, Guid metadataSchemaGuid, string languageCode, uint revisionID, XDocument metadataXml, Guid userGuid);

    IList<Format> FormatGet(uint? id = null, string name = null);
    uint FormatCreate(uint? formatCategoryID, string name, XDocument formatXml, string mimeType, string extension);

    uint ObjectTypeDelete(uint id);
    uint ObjectTypeSet(string name, uint? id = null);
    IList<ObjectType> ObjectTypeGet(uint? id, string name);

    uint FileDelete(uint id);
    uint FileCreate(Guid objectGuid, uint? parentID, uint destinationID, string filename, string originalFilename, string folderPath, uint formatID);
    IEnumerable<File> FileGet(uint? id = null, uint? parentId = null);
    File FileGet(uint id);

    uint LinkCreate(Guid objectGuid, uint folderID, int objectFolderTypeID);
    uint LinkUpdate(Guid objectGuid, uint folderID, uint newFolderID);
    uint LinkDelete(Guid objectGuid, uint folderID);

    IList<DestinationInfo> DestinationGet(uint? id);
    IFileRepository File { get; set; }
  }
}
