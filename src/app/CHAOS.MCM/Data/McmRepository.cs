using System.Threading;
using Chaos.Mcm.Data.MySql;
using MySql.Data.MySqlClient;

namespace Chaos.Mcm.Data
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml.Linq;
  using CHAOS.Data;
  using CHAOS.Data.MySql;
  using CHAOS.Extensions;
  using Dto;
  using Dto.Standard;
  using Mapping;
  using Exception;
  using Permission;
  using Portal.Core.Exceptions;
  using FolderPermission = Dto.FolderPermission;
  using Object = Dto.Object;

  // todo: Remove dependency to MySql
  public class McmRepository : IMcmRepository
  {
    private Gateway Gateway { get; set; }
    public IFileRepository File { get; set; }
    public IProjectRepository Project { get; set; }
		public ILabelRepository Label { get; set; }

    static McmRepository()
    {
      ReaderExtensions.Mappings.Add(typeof (File), new FileMapping());
      ReaderExtensions.Mappings.Add(typeof (Folder), new FolderMapping());
      ReaderExtensions.Mappings.Add(typeof (Format), new FormatMapping());
      ReaderExtensions.Mappings.Add(typeof (Object), new ObjectMapping());
      ReaderExtensions.Mappings.Add(typeof (Metadata), new MetadataMapping());
      ReaderExtensions.Mappings.Add(typeof (FileInfo), new FileInfoMapping());
      ReaderExtensions.Mappings.Add(typeof (ObjectType), new ObjectTypeMapping());
      ReaderExtensions.Mappings.Add(typeof (FolderInfo), new FolderInfoMapping());
      ReaderExtensions.Mappings.Add(typeof (AccessPoint), new AccessPointMapping());
      ReaderExtensions.Mappings.Add(typeof (ObjectFolder), new ObjectFolderMapping());
      ReaderExtensions.Mappings.Add(typeof (ObjectMetadata), new ObjectMetadataMapping());
      ReaderExtensions.Mappings.Add(typeof (MetadataSchema), new MetadataSchemaMapping());
      ReaderExtensions.Mappings.Add(typeof (DestinationInfo), new DestinationInfoMapping());
      ReaderExtensions.Mappings.Add(typeof (FolderPermission), new FolderPermissionMapping());
      ReaderExtensions.Mappings.Add(typeof (ObjectAccessPoint), new AccesspointObjectJoinMapping());
      ReaderExtensions.Mappings.Add(typeof (ObjectRelationInfo), new ObjectRelationInfoMapping());
    }

    public IMcmRepository WithConfiguration(string connectionString)
    {
      Gateway = new Gateway(connectionString);

      File = new FileRepository(Gateway);
	    Label = new LabelRepository(Gateway);
	    Project = new ProjectRepository(Gateway, Label);
      return this;
    }

    #region Business Logic

    #region Object Relation

    public uint ObjectRelationDelete(Guid object1Guid, Guid object2Guid, uint objectRelationTypeID)
    {
      var result = Gateway.ExecuteNonQuery("ObjectRelation_Delete", new[]
        {
          new MySqlParameter("Object1Guid", object1Guid.ToByteArray()),
          new MySqlParameter("Object2Guid", object2Guid.ToByteArray()),
          new MySqlParameter("ObjectRelationTypeID", objectRelationTypeID)
        });

      return (uint) result;
    }

    public IList<ObjectRelationInfo> ObjectRelationInfoGet(Guid objectGuid)
    {
      return this.Gateway.ExecuteQuery<ObjectRelationInfo>("ObjectRelationInfo_Get",
                                                           new MySqlParameter("Object1Guid", objectGuid.ToByteArray()));
    }

    public uint ObjectRelationSet(Guid object1Guid, Guid object2Guid, uint objectRelationTypeID, int? sequence)
    {
      var result = this.Gateway.ExecuteNonQuery("ObjectRelation_Set", new[]
        {
          new MySqlParameter("Object1Guid", object1Guid.ToByteArray()),
          new MySqlParameter("Object2Guid", object2Guid.ToByteArray()),
          new MySqlParameter("ObjectRelationTypeID", objectRelationTypeID),
          new MySqlParameter("Sequence", sequence)
        });

      if (result == -100)
        throw new InsufficientPermissionsException("The user do not have permission to create object relations");
      if (result == -200) throw new ObjectRelationAlreadyExistException("The object relation already exists");

      return (uint) result;
    }

    public uint ObjectRelationSet(Guid object1Guid, Guid object2Guid, uint objectRelationTypeID, int? sequence,
                                  Guid metadataGuid, Guid metadataSchemaGuid, string languageCode, XDocument metadataXml,
                                  Guid editingUserGuid)
    {
      var result = Gateway.ExecuteNonQuery("ObjectRelation_SetMetadata", new[]
        {
          new MySqlParameter("Object1Guid", object1Guid.ToByteArray()),
          new MySqlParameter("Object2Guid", object2Guid.ToByteArray()),
          new MySqlParameter("ObjectRelationTypeID", objectRelationTypeID),
          new MySqlParameter("Sequence", sequence),
          new MySqlParameter("MetadataGuid", metadataGuid.ToByteArray()),
          new MySqlParameter("MetadataSchemaGuid", metadataSchemaGuid.ToByteArray()),
          new MySqlParameter("MetadataXml", metadataXml.ToString()),
          new MySqlParameter("LanguageCode", languageCode),
          new MySqlParameter("EditingUserGuid", editingUserGuid.ToByteArray()),
        });

      if (result == -100)
        throw new InsufficientPermissionsException("The user do not have permission to create object relations");
      if (result == -200) throw new ObjectRelationAlreadyExistException("The object relation already exists");

      return (uint) result;
    }

    #endregion

    #region Metadata

    public IEnumerable<Metadata> MetadataGet(Guid guid)
    {
      return Gateway.ExecuteQuery<Metadata>("Metadata_Get", new MySqlParameter("Guid", guid.ToByteArray()));
    }

    public uint MetadataSet(Guid objectGuid, Guid metadataGuid, Guid metadataSchemaGuid, string languageCode,
                            uint revisionID, XDocument metadataXml, Guid editingUserGuid)
    {
      var result = Gateway.ExecuteNonQuery("Metadata_Set", new[]
        {
          new MySqlParameter("Guid", metadataGuid.ToByteArray()),
          new MySqlParameter("ObjectGuid", objectGuid.ToByteArray()),
          new MySqlParameter("MetadataSchemaGUID", metadataSchemaGuid.ToByteArray()),
          new MySqlParameter("LanguageCode", languageCode),
          new MySqlParameter("RevisionID", revisionID),
          new MySqlParameter("MetadataXML", metadataXml.ToString()),
          new MySqlParameter("EditingUserGUID", editingUserGuid.ToByteArray())
        });

      if (result == -200) throw new UnhandledException("Metadata set failed on the database and was rolled back");
      if (result == -201) throw new InvalidRevisionException("The provided Revision ID does not match the latest in the database");

      return (uint) result;
    }

    #endregion

    #region Object

    public uint ObjectDelete(Guid guid)
    {
      var result = Gateway.ExecuteNonQuery("Object_Delete", new[]
        {
          new MySqlParameter("Guid", guid.ToByteArray())
        });

      if (result == -200) throw new UnhandledException("Object was not deleted, database rolled back");

      return (uint) result;
    }

    public uint ObjectCreate(Guid guid, uint objectTypeID, uint folderID)
    {
      var result = Gateway.ExecuteNonQuery("Object_Create", new[]
        {
          new MySqlParameter("Guid", guid.ToByteArray()),
          new MySqlParameter("ObjectTypeID", objectTypeID),
          new MySqlParameter("FolderID", folderID)
        });

      if (result == -1) throw new ArgumentException("Guid already exist");
      if (result == -200) throw new UnhandledException("Unhandled exception, Create was rolled back");

      return (uint) result;
    }

    public IList<Object> ObjectGet(uint? folderID = null, uint pageIndex = 0, uint pageSize = 5,
                                   bool includeMetadata = false, bool includeFiles = false,
                                   bool includeObjectRelations = false, bool includeFolders = false,
                                   bool includeAccessPoints = false, uint? objectTypeId = null)
    {
				return TryExecute<Object>("Object_GetByFolderID", 10,
					new MySqlParameter("FolderID", folderID),
					new MySqlParameter("ObjectTypeId", objectTypeId),
					new MySqlParameter("PageIndex", pageIndex),
					new MySqlParameter("PageSize", pageSize),
					new MySqlParameter("IncludeMetadata", includeMetadata),
					new MySqlParameter("IncludeFiles", includeFiles),
					new MySqlParameter("IncludeObjectRelations", includeObjectRelations),
					new MySqlParameter("IncludeFolders", includeFolders),
					new MySqlParameter("IncludeAccessPoints", includeAccessPoints));
    }

		public IList<T> TryExecute<T>(string sp, uint retries, params MySqlParameter[] parameters)
		{
			try
			{
				return Gateway.ExecuteQuery<T>(sp, parameters);
			}
			catch (Exception)
			{
				if (retries <= 0) throw; 

				Thread.Sleep(100);

				return TryExecute<T>(sp, --retries, parameters);
			}
		}

		public Object ObjectGet(Guid objectGuid, bool includeMetadata = false, bool includeFiles = false,
                            bool includeObjectRelations = false, bool includeFolders = false,
                            bool includeAccessPoints = false)
    {
      return
        ObjectGet(new[] {objectGuid}, includeMetadata, includeFiles, includeObjectRelations, includeFolders,
                  includeAccessPoints).FirstOrDefault();
    }

    public IList<Object> ObjectGet(IEnumerable<Guid> objectGuids, bool includeMetadata = false,
                                   bool includeFiles = false, bool includeObjectRelations = false,
                                   bool includeFolders = false, bool includeAccessPoints = false)
    {
      var guids = GuidListToString(objectGuids);

      return Gateway.ExecuteQuery<Object>("Object_GetByGUIDs", new[]
        {
          new MySqlParameter("GUIDs", guids),
          new MySqlParameter("IncludeMetadata", includeMetadata),
          new MySqlParameter("IncludeFiles", includeFiles),
          new MySqlParameter("IncludeObjectRelations", includeObjectRelations),
          new MySqlParameter("IncludeFolders", includeFolders),
          new MySqlParameter("IncludeAccessPoints", includeAccessPoints)
        });
    }

    private static string GuidListToString(IEnumerable<Guid> objectGuids)
    {
      var guids = String.Join(",", objectGuids.Select(item => item.ToUUID().ToString().Replace("-", "")));
      return guids;
    }

    #endregion

    #region Folder

    public IList<Folder> FolderGet(uint? id = null, Guid? userGuid = null, Guid? objectGuid = null)
    {
      if (userGuid.HasValue) throw new NotImplementedException("Folder get by userGuid is not implemented");

      return Gateway.ExecuteQuery<Folder>("Folder_Get", new[]
        {
          new MySqlParameter("ID", id),
          new MySqlParameter("ObjectGuid", objectGuid.HasValue ? objectGuid.Value.ToByteArray() : null),
        });
    }

    public IList<FolderInfo> FolderInfoGet(IEnumerable<uint> ids)
    {
      if (!ids.Any()) return new List<FolderInfo>();

      return Gateway.ExecuteQuery<FolderInfo>("FolderInfo_Get", new MySqlParameter("FolderIDs", string.Join(",", ids)));
    }

    public int FolderDelete(uint id)
    {
      var result = Gateway.ExecuteNonQuery("Folder_Delete", new MySqlParameter("ID", id));

      if (result == -200) throw new UnhandledException("An unknown error occured on folder_delete and was rolled back");
      if (result == -50) throw new FolderNotEmptyException("The folder has to be empty to be deleted");

      return (int) result;
    }

    public uint FolderCreate(Guid userGuid, Guid? subscriptionGuid, string name, uint? parentID, uint folderTypeID)
    {
      var result = Gateway.ExecuteNonQuery("Folder_Create", new[]
        {
          new MySqlParameter("UserGuid", userGuid.ToByteArray()),
          new MySqlParameter("SubscriptionGuid", subscriptionGuid.HasValue ? subscriptionGuid.Value.ToByteArray() : null)
          ,
          new MySqlParameter("Name", name),
          new MySqlParameter("ParentID", parentID),
          new MySqlParameter("FolderTypeID", folderTypeID),
        });

      if (result == -200) throw new UnhandledException("An unknown error occured on Folder_Create and was rolled back");
      if (result == -100)
        throw new InsufficientPermissionsException("User does not have permission to Create the folder");
      if (result == -10) throw new ArgumentException("Invalid input parameters");

      return (uint) result;
    }

    public uint FolderUpdate(uint id, string newName, uint? newParentID, uint? newFolderTypeID)
    {
      var result = Gateway.ExecuteNonQuery("Folder_Update", new[]
        {
          new MySqlParameter("ID", id),
          new MySqlParameter("NewName", newName),
          new MySqlParameter("NewParentID", newParentID),
          new MySqlParameter("NewFolderTypeID", newFolderTypeID),
        });

      return (uint) result;
    }

    #endregion

    #region Format

    public IList<Format> FormatGet(uint? id = null, string name = null)
    {
      return Gateway.ExecuteQuery<Format>("Format_Get", new[]
        {
          new MySqlParameter("ID", id),
          new MySqlParameter("Name", name),
        });
    }

    public uint FormatCreate(uint? formatCategoryID, string name, XDocument formatXml, string mimeType, string extension)
    {
      var result = Gateway.ExecuteNonQuery("Format_Create", new[]
        {
          new MySqlParameter("Name", name),
          new MySqlParameter("FormatCategoryID", formatCategoryID),
          new MySqlParameter("FormatXml", formatXml),
          new MySqlParameter("MimeType", mimeType),
          new MySqlParameter("Extension", extension),
        });

      return (uint) result;
    }

    #endregion

    #region ObjectType

    public uint ObjectTypeDelete(uint id)
    {
      var result = Gateway.ExecuteNonQuery("ObjectType_Delete", new[]
        {
          new MySqlParameter("ID", id),
          new MySqlParameter("Name", null)
        });

      if (result == -100)
        throw new InsufficientPermissionsException("User does not have permission to delete an Object Type");

      return (uint) result;
    }

    public uint ObjectTypeSet(string name, uint? id = null)
    {
      var result = Gateway.ExecuteNonQuery("ObjectType_Set", new[]
        {
          new MySqlParameter("ID", id),
          new MySqlParameter("Name", name)
        });

      return (uint) result;
    }

    public IList<ObjectType> ObjectTypeGet(uint? id, string name)
    {
      return Gateway.ExecuteQuery<ObjectType>("ObjectType_Get", new[]
        {
          new MySqlParameter("ID", id),
          new MySqlParameter("Name", name),
        });
    }

    #endregion

    #region Metadata Schema

    public IList<MetadataSchema> MetadataSchemaGet(Guid userGuid, IEnumerable<Guid> groupGuids, Guid? metadataSchemaGuid, MetadataSchemaPermission permission)
    {
      var guids = String.Join(",", groupGuids.Select(item => item.ToString().Replace("-", "")));

      return Gateway.ExecuteQuery<MetadataSchema>("MetadataSchema_Get", new[]
        {
          new MySqlParameter("UserGuid", userGuid.ToByteArray()),
          new MySqlParameter("GroupGuids", guids),
          new MySqlParameter("MetadataSchemaGuid", metadataSchemaGuid.HasValue ? metadataSchemaGuid.Value.ToByteArray() : null),
          new MySqlParameter("PermissionRequired", (int?) permission),
        });
    }
    
    public IList<MetadataSchema> MetadataSchemaGet(Guid? metadataSchemaGuid = null)
    {
      return Gateway.ExecuteQuery<MetadataSchema>("MetadataSchema_Get", new[]
        {
          new MySqlParameter("UserGuid", null),
          new MySqlParameter("GroupGuids", null),
          new MySqlParameter("MetadataSchemaGuid", metadataSchemaGuid.HasValue ? metadataSchemaGuid.Value.ToByteArray() : null),
          new MySqlParameter("PermissionRequired", null),
        });
    }

    public uint MetadataSchemaUpdate(string name, string schema, Guid userGuid, Guid guid)
    {
      return MetadataSchemaUpdate(new MetadataSchema {Guid = guid, Name = name, Schema = schema});
    }

    public uint MetadataSchemaUpdate(MetadataSchema schema)
    {
      var result = Gateway.ExecuteNonQuery("MetadataSchema_Update", new[]
        {
          new MySqlParameter("Guid", schema.Guid.ToByteArray()),
          new MySqlParameter("Name", schema.Name),
          new MySqlParameter("SchemaXml", schema.Schema)
        });

      return (uint)result;
    }

    public uint MetadataSchemaCreate(string name, string schema, Guid userGuid, Guid guid)
    {
      return MetadataSchemaCreate(new MetadataSchema {Guid = guid, Name = name, Schema = schema}, userGuid);
    }

    public uint MetadataSchemaCreate(MetadataSchema schema, Guid userGuid)
    {
      var result = Gateway.ExecuteNonQuery("MetadataSchema_Create", new[]
        {
          new MySqlParameter("Guid", schema.Guid.ToByteArray()),
          new MySqlParameter("Name", schema.Name),
          new MySqlParameter("SchemaXml", schema.Schema),
          new MySqlParameter("UserGuid", userGuid.ToByteArray())
        });

      if (result == -200)
        throw new UnhandledException("MetadataSchema_Create failed on the database, and was rolled back");

      return (uint)result;
    }
    
    public uint MetadataSchemaDelete(Guid guid)
    {
      var result = Gateway.ExecuteNonQuery("MetadataSchema_Delete", new MySqlParameter("Guid", guid.ToByteArray()));

      if (result == -200)
        throw new UnhandledException("MetadataSchema_Delete failed on the database, and was rolled back");

      return (uint) result;
    }

    #endregion

    #region Folder Permission

    public IList<FolderPermission> FolderPermissionGet()
    {
      return Gateway.ExecuteQuery<FolderPermission>("FolderPermission_Get");
    }

    public uint FolderUserJoinSet(Guid userGuid, uint folderID, uint permission)
    {
      var result = Gateway.ExecuteNonQuery("Folder_User_Join_Set", new[]
        {
          new MySqlParameter("UserGuid", userGuid.ToByteArray()),
          new MySqlParameter("FolderID", folderID),
          new MySqlParameter("Permission", permission)
        });

      return (uint) result;
    }

    public uint FolderGroupJoinSet(Guid groupGuid, uint folderID, uint permission)
    {
      var result = Gateway.ExecuteNonQuery("Folder_Group_Join_Set", new[]
        {
          new MySqlParameter("GroupGuid", groupGuid.ToByteArray()),
          new MySqlParameter("FolderID", folderID),
          new MySqlParameter("Permission", permission)
        });

      return (uint) result;
    }

    #endregion

    #region AccessPoint

    public IList<AccessPoint> AccessPointGet(Guid accessPointGuid, Guid userGuid, IEnumerable<Guid> groupGuids,
                                             uint permission)
    {
      var guids = GuidListToString(groupGuids);

      return Gateway.ExecuteQuery<AccessPoint>("AccessPoint_Get", new[]
        {
          new MySqlParameter("AccessPointGuid", accessPointGuid.ToByteArray()),
          new MySqlParameter("UserGuid", userGuid.ToByteArray()),
          new MySqlParameter("GroupGuids", guids),
          new MySqlParameter("Permission", permission)
        });
    }

    public uint AccessPointPublishSettingsSet(Guid accessPointGuid, Guid objectGuid, DateTime? startDate,
                                              DateTime? endDate)
    {
      var result = Gateway.ExecuteNonQuery("AccessPoint_Object_Join_Set", new[]
        {
          new MySqlParameter("AccessPointGuid", accessPointGuid.ToByteArray()),
          new MySqlParameter("ObjectGuid", objectGuid.ToByteArray()),
          new MySqlParameter("StartDate", startDate),
          new MySqlParameter("EndDate", endDate)
        });

      return (uint) result;
    }

    #endregion

    #region Link

    public uint LinkCreate(Guid objectGuid, uint folderID, int objectFolderTypeID)
    {
      var result = Gateway.ExecuteNonQuery("Object_Folder_Join_Create", new[]
        {
          new MySqlParameter("ObjectGuid", objectGuid.ToByteArray()),
          new MySqlParameter("FolderID", folderID),
          new MySqlParameter("ObjectFolderTypeID", objectFolderTypeID),
        });

      if (result == -100) throw new InsufficientPermissionsException("User can only create links");

      return (uint) result;
    }

    public uint LinkUpdate(Guid objectGuid, uint folderID, uint newFolderID)
    {
      var result = Gateway.ExecuteNonQuery("Object_Folder_Join_Update", new[]
        {
          new MySqlParameter("ObjectGuid", objectGuid.ToByteArray()),
          new MySqlParameter("FolderID", folderID),
          new MySqlParameter("NewFolderID", newFolderID),
        });

      if (result == -100) throw new InsufficientPermissionsException("User can only create links");

      return (uint) result;
    }

    public uint LinkDelete(Guid objectGuid, uint folderID)
    {
      var result = Gateway.ExecuteNonQuery("Object_Folder_Join_Delete", new[]
        {
          new MySqlParameter("ObjectGuid", objectGuid.ToByteArray()),
          new MySqlParameter("FolderID", folderID),
        });

      if (result == -100) throw new InsufficientPermissionsException("User can only create links");

      return (uint) result;
    }

    #endregion

    #region Destination

    public IList<DestinationInfo> DestinationGet(uint? id)
    {
      return Gateway.ExecuteQuery<DestinationInfo>("DestinationInfo_Get", new MySqlParameter("ID", id));
    }

    #endregion

    #region File

    public uint FileDelete(uint id)
    {
      return File.Delete(id);
    }

    public uint FileCreate(Guid objectGuid, uint? parentID, uint destinationID, string filename, string originalFilename,
                           string folderPath, uint formatID)
    {
      return File.Create(objectGuid, parentID, destinationID, filename, originalFilename, folderPath, formatID);
    }

    public uint FileSet(File file)
    {
      return File.Set(file);
    }

    public File FileGet(uint id)
    {
      return File.Get(id);
    }

    public IEnumerable<File> FileGet(uint? id = null, uint? parentId = null)
    {
      return File.Get(id, parentId);
    }

    #endregion

    #endregion
  }
}