﻿namespace Chaos.Mcm.Data
{
    using System;
    using System.Xml.Linq;
    using System.Collections.Generic;
    
    using Chaos.Mcm.Data.Dto;
    using Chaos.Mcm.Data.Dto.Standard;
    using Chaos.Mcm.Permission;

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

        uint ObjectCreate(Guid guid, uint objectTypeID, uint folderID);
        NewObject ObjectGet(Guid objectGuid, bool includeMetadata = false, bool includeFiles = false, bool includeObjectRelations = false, bool includeFolders = false, bool includeAccessPoints = false);
        IList<NewObject> ObjectGet( IEnumerable<Guid> objectGuids, bool includeMetadata = false, bool includeFiles = false, bool includeObjectRelations = false, bool includeFolders = false, bool includeAccessPoints = false );
        IList<NewObject> ObjectGet(uint? folderID = null, uint pageIndex = 0, uint pageSize = 5, bool includeMetadata = false, bool includeFiles = false, bool includeObjectRelations = false, bool includeFolders = false, bool includeAccessPoints = false);

        uint ObjectRelationSet(Guid object1Guid, Guid object2Guid, uint objectRelationTypeID, int? sequence);
        uint ObjectRelationSet(ObjectRelationInfo objectRelationInfo, Guid editingUserGuid);

        IEnumerable<NewMetadata> MetadataGet(Guid guid);

        uint MetadataSet(Guid objectGuid, Guid metadataGuid, Guid metadataSchemaGuid, string languageCode, uint revisionID, XDocument metadataXml, Guid userGuid);

        IList<Folder> FolderGet(Guid? userGuid = null, Guid? objectGuid = null);

        IList<Format> FormatGet(uint? id = null, string name = null);
        uint FormatCreate(uint? formatCategoryID, string name, XDocument formatXml, string mimeType, string extension);

        uint ObjectTypeDelete(uint id);
        uint ObjectTypeSet(string name);
        IList<ObjectType> ObjectTypeGet(uint? expectedID, string name);

        uint FileDelete(uint id);
        uint FileCreate(Guid objectGuid, uint? parentID, uint destinationID, string filename, string originalFilename, string folderPath, uint formatID);
        IList<File> FileGet(uint id);

        uint MetadataSchemaSet(string name, XDocument schemaXml, Guid userGuid, Guid guid);
        uint MetadataSchemaDelete(Guid guid);
        IEnumerable<MetadataSchema> MetadataSchemaGet(Guid userGuid, IEnumerable<Guid> groupGuids, Guid? metadataSchemaGuid, MetadataSchemaPermission permission);

        uint ObjectDelete(Guid guid);
    }
}
