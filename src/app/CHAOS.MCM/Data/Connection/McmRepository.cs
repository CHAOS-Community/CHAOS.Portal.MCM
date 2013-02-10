namespace Chaos.Mcm.Data.Connection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using CHAOS.Extensions;

    using Chaos.Mcm.Data.Connection.MySql;
    using Chaos.Mcm.Data.Dto;
    using Chaos.Mcm.Data.Dto.Standard;
    using Chaos.Mcm.Data.EF;
    using Chaos.Mcm.Exception;
    using Chaos.Mcm.Permission;
    using Chaos.Portal.Exceptions;

    using global::MySql.Data.MySqlClient;

    using File = Chaos.Mcm.Data.Dto.Standard.File;
    using Folder = Chaos.Mcm.Data.Dto.Standard.Folder;
    using Format = Chaos.Mcm.Data.Dto.Standard.Format;
    using ObjectType = Chaos.Mcm.Data.Dto.Standard.ObjectType;

    // todo: Remove dependency to MySql
    public class McmRepository : IMcmRepository
    {
        #region Fields

        private string _connectionString;

        #endregion
        #region Properties

        private Gateway Gateway { get; set; }

        #endregion
        #region Construction

        public IMcmRepository WithConfiguration(string connectionString)
        {
            _connectionString = connectionString;
            Gateway           = new Gateway(connectionString);
            return this;
        }

        private MCMEntities CreateMcmEntities()
        {
            return new MCMEntities(this._connectionString);
        }

        #endregion
        #region Business Logic

        #region Metadata

        public IEnumerable<NewMetadata> MetadataGet(Guid guid)
        {
            return this.Gateway.ExecuteQuery<NewMetadata>("Metadata_Get", new MySqlParameter("Guid", guid.ToByteArray()));
        }

        public uint MetadataSet(Guid objectGuid, Guid metadataGuid, Guid metadataSchemaGuid, string languageCode, uint revisionID, XDocument metadataXml, Guid editingUserGuid)
        {
            var result = this.Gateway.ExecuteNonQuery("Metadata_Set", new[]
                {
                    new MySqlParameter("Guid", metadataGuid.ToByteArray()),
                    new MySqlParameter("ObjectGuid", objectGuid.ToByteArray()),
                    new MySqlParameter("MetadataSchemaGUID", metadataSchemaGuid.ToByteArray()),
                    new MySqlParameter("LanguageCode", languageCode),
                    new MySqlParameter("RevisionID", revisionID),
                    new MySqlParameter("MetadataXML", metadataXml),
                    new MySqlParameter("EditingUserGUID", editingUserGuid.ToByteArray())
                });

            if (result == -200) throw new UnhandledException("NewMetadata set failed on the database and was rolled back");

            return (uint)result;
        }

        #endregion
        #region Folder

        public int DeleteFolder(uint id)
        {
            using (var db = this.CreateMcmEntities())
            {
                var result = db.Folder_Delete((int?) id).FirstOrDefault();

                if(result.HasValue && result.Value == -200)
                    throw new UnhandledException("An unknown error occured on folder_delete and was rolled back");

                if(result.HasValue && result.Value == -50)
                    throw new InsufficientPermissionsException("The folder has to be empty to be deleted");

                return result.Value;
            }
        }

        public uint CreateFolder(Guid userGuid, Guid? subscriptionGuid, string title, uint? parentID, uint folderTypeID )
        {
            using (var db = this.CreateMcmEntities())
            {
                var result = db.Folder_Create(userGuid.ToByteArray(),
                                              subscriptionGuid.HasValue ? subscriptionGuid.Value.ToByteArray() : null,
                                              title,
                                              (int?) parentID,
                                              (int?) folderTypeID).FirstOrDefault();

                if(result.HasValue && result == -200)
                    throw new UnhandledException("An unknown error occured on Folder_Create and was rolled back");

                if(result.HasValue && result == -10)
                    throw new UnhandledException("Invalid input parameters");

                return (uint) result.Value;
            }
        }

        public uint UpdateFolder(uint id, string newTitle, uint? newParentID, uint? newFolderTypeID)
        {
            using (var db = this.CreateMcmEntities())
            {
                var result = db.Folder_Update((int)id, newTitle, (int?)newParentID, (int?)newFolderTypeID).FirstOrDefault();

                if (!result.HasValue)
                    throw new UnhandledException("Folder_Update finished without a value");

                return (uint)result;
            }
        }

        public IList<Folder> FolderGet(Guid? userGuid = null, Guid? objectGuid = null)
        {
            using (var db = DefaultMCMEntities)
            {
                var folders = db.Folder_Get(null, objectGuid.ToByteArray()).Select(item => PermissionManager.GetFolders((uint)item.ID));


                return PermissionManager.DoesUserOrGroupHavePermissionToFolders(userGUID, groupGUIDs, permissions, folders);
            }
        }

        public IList<Format> FormatGet(uint? id = null, string name = null)
        {
            using (var db = DefaultMCMEntities)
            {
                return db.Format_Get((int?)ID, name).ToDto().ToList();
            }
        }

        public uint FormatCreate(uint? formatCategoryID, string name, XDocument formatXml, string mimeType, string extension)
        {
            using (var db = DefaultMCMEntities)
            {
                var result = db.Format_Create((int?)formatCategoryID, name, formatXml, mimeType, extension).FirstOrDefault();

                if (result == null)
                    throw new UnhandledException("No result was received from the database");

                return new result.Value;
            }
        }

        public uint ObjectTypeDelete(uint id)
        {
            using (var db = DefaultMCMEntities)
            {
                var result = db.ObjectType_Delete((int?)id, null).First();

                if (result.Value == -100)
                    throw new InsufficientPermissionsException("User does not have permission to delete an Object Type");

                return result.Value;
            }
        }

        public uint ObjectTypeSet(string name)
        {
            using (var db = DefaultMCMEntities)
            {
                return db.ObjectType_Create(name).First().Value;
            }

        }

        public IList<ObjectType> ObjectTypeGet(uint? expectedID, string name)
        {
            using (var db = DefaultMCMEntities)
            {
                return db.ObjectType_Get(result, null).ToDto().First();
            }
        }

        public uint FileDelete(uint id)
        {
            using (var db = DefaultMCMEntities)
            {
                var result = db.File_Delete((int?)id).FirstOrDefault();

                if (!result.HasValue)
                    throw new UnhandledException("File delete failed in the database and was rolled back");

                return result.Value;
            }
        }

        #endregion
        #region NewMetadata Schema

        public IEnumerable<Dto.Standard.MetadataSchema> MetadataSchemaGet(Guid userGuid, IEnumerable<Guid> groupGuids, Guid? metadataSchemaGuid, MetadataSchemaPermission permission )
        {
            using( var db = this.CreateMcmEntities() )
            {
                var sGroupGuids = string.Join(",", groupGuids.Select(guid => guid.ToUUID().ToString().Replace("-", "")));

                return db.MetadataSchema_Get(userGuid.ToByteArray(), sGroupGuids, metadataSchemaGuid.HasValue ? metadataSchemaGuid.Value.ToByteArray() : null, (int?)permission).ToList().ToDto();
			}
        }

        public uint MetadataSchemaSet(string name, XDocument schemaXml, Guid userGuid, Guid guid)
        {
            using (var db = DefaultMCMEntities)
            {
                var result = db.MetadataSchema_Set(guid.ToByteArray(), name, schemaXml, callContext.User.Guid.ToByteArray()).FirstOrDefault();

                if (!result.HasValue || result.Value != 1)
                    throw new UnhandledException("MetadataSchema was not created");

                return result;
            }
        }

        public uint MetadataSchemaDelete(Guid guid)
        {
            using (var db = DefaultMCMEntities)
            {
                var result = db.MetadataSchema_Delete(guid.ToByteArray()).FirstOrDefault();

                if (result == null || !result.HasValue || result.Value != 1)
                    throw new UnhandledException("MetadataSchema was not deleted");

                return new ScalarResult(result.Value);
            }
        }

        #endregion
        #region Object Relation

        public IList<ObjectRelationInfo> ObjectRelationInfoGet(Guid objectGuid)
        {
            return this.Gateway.ExecuteQuery<ObjectRelationInfo>("ObjectRelationInfo_Get", new MySqlParameter("Object1Guid", objectGuid.ToByteArray()));
        }

        public uint ObjectRelationSet(Guid object1Guid, Guid object2Guid, uint objectRelationTypeID, int? sequence)
        {
            var result = this.Gateway.ExecuteNonQuery("ObjectRelation_Set",new[]
                    {
                        new MySqlParameter("Object1Guid", object1Guid.ToByteArray()),
                        new MySqlParameter("Object2Guid", object2Guid.ToByteArray()),
                        new MySqlParameter("ObjectRelationTypeID", objectRelationTypeID),
                        new MySqlParameter("Sequence", sequence)
                    });

            if (result == -100)
                throw new InsufficientPermissionsException("The user do not have permission to create object relations");

            if (result == -200)
                throw new ObjectRelationAlreadyExistException("The object relation already exists");

            return (uint)result;
        }

        public uint ObjectRelationSet(ObjectRelationInfo objectRelationInfo, Guid editingUserGuid)
        {
            var result = this.Gateway.ExecuteNonQuery("ObjectRelation_SetMetadata", new[]
                {
                    new MySqlParameter("Object1Guid", objectRelationInfo.Object1Guid.ToByteArray()),
                    new MySqlParameter("Object2Guid", objectRelationInfo.Object2Guid.ToByteArray()),
                    new MySqlParameter("ObjectRelationTypeID", objectRelationInfo.ObjectRelationTypeID),
                    new MySqlParameter("Sequence", objectRelationInfo.Sequence),
                    new MySqlParameter("MetadataGuid", objectRelationInfo.MetadataGuid.Value.ToByteArray()),
                    new MySqlParameter("MetadataSchemaGuid", objectRelationInfo.MetadataSchemaGuid.Value.ToByteArray()),
                    new MySqlParameter("MetadataXml", objectRelationInfo.MetadataXml),
                    new MySqlParameter("LanguageCode", objectRelationInfo.LanguageCode),
                    new MySqlParameter("EditingUserGuid", editingUserGuid.ToByteArray()),
                });

            if (result == -100)
                throw new InsufficientPermissionsException("The user do not have permission to create object relations");

            if (result == -200)
                throw new ObjectRelationAlreadyExistException("The object relation already exists");

            return (uint)result;
        }

        #endregion
        #region Folder User Join

        public IEnumerable<FolderUserJoin> GetFolderUserJoin()
        {
            using(var db = this.CreateMcmEntities())
            {
                return
                    db.Folder_User_Join.ToList().Select(
                        item =>
                        new FolderUserJoin
                            {
                                FolderID = (uint)item.FolderID,
                                UserGuid = item.UserGUID,
                                Permission = (uint)item.Permission,
                                DateCreated = item.DateCreated
                            });
            }
        }

        public uint SetFolderUserJoin(Guid userGuid, uint folderID, uint permission)
        {
            using(var db = this.CreateMcmEntities())
            {
                var result =
                    db.Folder_User_Join_Set(userGuid.ToByteArray(), (int?)folderID, (int?)permission).FirstOrDefault();

                if(!result.HasValue) throw new UnhandledException("Folder_User_Join_Set failed on the database and was rolled back");

                return (uint)result.Value;
            }
        }

        #endregion
        #region Folder

        public IEnumerable<FolderGroupJoin> GetFolderGroupJoin()
        {
            using(var db = this.CreateMcmEntities())
            {
                return
                    db.Folder_Group_Join.ToList().Select(
                        item =>
                        new FolderGroupJoin
                            {
                                FolderID = (uint)item.FolderID,
                                GroupGuid = item.GroupGUID,
                                Permission = (uint)item.Permission,
                                DateCreated = item.DateCreated
                            });
            }
        }

        public uint SetFolderGroupJoin(Guid groupGuid, uint folderID, uint permission)
        {
            using(var db = this.CreateMcmEntities())
            {
                var result =
                    db.Folder_Group_Join_Set(groupGuid.ToByteArray(), (int?)folderID, (int?)permission).FirstOrDefault();

                if(!result.HasValue) throw new UnhandledException("Folder_Group_Join_Set failed on the database and was rolled back");

                return (uint)result.Value;
            }
        }

        public IEnumerable<Folder> GetFolder()
        {
            using(var db = this.CreateMcmEntities())
            {
                return db.Folder_Get(null, null).ToList().ToDto();
            }
        }

        public IEnumerable<IFolderInfo> GetFolderInfo(IEnumerable<uint> ids)
        {
            var folderIDs = ids.Select(item => (long)item);
            var folderIDStrings = string.Join(",", ids);

            // TODO: optimize folder retrival form the database
            using(var db = this.CreateMcmEntities())
            {
                return db.FolderInfo.Where(fi => folderIDs.Contains(fi.ID)).ToList().ToDto();
            }
        }

        #endregion
        #region AccessPoint

        public IEnumerable<Dto.Standard.AccessPoint> GetAccessPoint(Guid accessPointGuid, Guid userGuid, IEnumerable<Guid> groupGuids, uint permission)
        {
            var groupGuidsString = string.Join(",", groupGuids);

            using (var db = this.CreateMcmEntities())
            {
                return db.AccessPoint_Get(accessPointGuid.ToByteArray(), userGuid.ToByteArray(), groupGuidsString, (int?)permission).ToList().ToDto();
            }
        }

        public uint SetAccessPointPublishSettings( Guid accessPointGuid, Guid objectGuid, DateTime? startDate, DateTime? endDate )
        {
            using (var db = this.CreateMcmEntities())
            {
                var result = db.AccessPoint_Object_Join_Set(accessPointGuid.ToByteArray(), objectGuid.ToByteArray(), startDate, endDate).FirstOrDefault();

                if(!result.HasValue)
                    throw new UnhandledException("SetAccessPointPublishSettings failed on the database, and was rolled back");

                return (uint) result.Value;
            }
        }

        #endregion
        #region Object

        public uint ObjectDelete(Guid guid)
        {
            using (var db = DefaultMCMEntities)
            {
                var result = db.Object_Delete(guid.ToByteArray()).FirstOrDefault();

                if (!result.HasValue || result.Value == -200)
                    throw new UnhandledException("Object was not deleted, database rolled back");

                return result;
            }
        }

        public uint ObjectCreate(Guid guid, uint objectTypeID, uint folderID)
        {
            using (var db = DefaultMCMEntities)
            {
                var result = db.Object_Create(guid.ToByteArray(), (int)objectTypeID, (int)folderID).FirstOrDefault();

                if (result.HasValue && result.Value == -200)
                    throw new UnhandledException("Unhandled exception, Set was rolled back");

                return (uint)result;
            }
        }

        public IList<NewObject> ObjectGet(uint? folderID = null, uint pageIndex = 0, uint pageSize = 5, bool includeMetadata = false, bool includeFiles = false, bool includeObjectRelations = false, bool includeFolders = false, bool includeAccessPoints = false)
        {
            return Gateway.ExecuteQuery<NewObject>("Object_GetByFolderID", new[]
                {
                    new MySqlParameter("FolderID", folderID),
                    new MySqlParameter("PageIndex", pageIndex),
                    new MySqlParameter("PageSize", pageSize),
                    new MySqlParameter("IncludeMetadata", includeMetadata),
                    new MySqlParameter("IncludeFiles", includeFiles),
                    new MySqlParameter("IncludeObjectRelations", includeObjectRelations),
                    new MySqlParameter("IncludeFolders", includeFolders),
                    new MySqlParameter("IncludeAccessPoints", includeAccessPoints)
                });
        }

        public NewObject ObjectGet(Guid objectGuid, bool includeMetadata = false, bool includeFiles = false, bool includeObjectRelations = false, bool includeFolders = false, bool includeAccessPoints = false)
        {
            return ObjectGet(new[] { objectGuid }, includeMetadata, includeFiles, includeObjectRelations, includeFolders, includeAccessPoints).FirstOrDefault();
        }

        public IList<NewObject> ObjectGet(IEnumerable<Guid> objectGuids, bool includeMetadata = false, bool includeFiles = false, bool includeObjectRelations = false, bool includeFolders = false, bool includeAccessPoints = false)
        {
            var guids = String.Join(",", objectGuids.Select(item => item.ToString().Replace("-", "")));

            return Gateway.ExecuteQuery<NewObject>("Object_GetByGUIDs", new[]
                {
                    new MySqlParameter("GUIDs", guids),
                    new MySqlParameter("IncludeMetadata", includeMetadata),
                    new MySqlParameter("IncludeFiles", includeFiles),
                    new MySqlParameter("IncludeObjectRelations", includeObjectRelations),
                    new MySqlParameter("IncludeFolders", includeFolders),
                    new MySqlParameter("IncludeAccessPoints", includeAccessPoints)
                });
        }

        #endregion
        #region File

        public uint FileCreate(Guid objectGuid, uint? parentID, uint destinationID, string filename, string originalFilename, string folderPath, uint formatID)
        {
            using (var db = DefaultMCMEntities)
            {
                var result = db.File_Create(objectGUID.ToByteArray(), (int?)parentFileID, (int)formatID, (int)destinationID, filename, originalFilename, folderPath).FirstOrDefault();

                if (!result.HasValue)
                    throw new UnhandledException("The creating the file failed in the database and was rolled back");

                return result;
            }
        }

        public IList<File> FileGet(uint id)
        {
            using (var db = DefaultMCMEntities)
            {
                return db.File_Get(result.Value).First().ToDto();
            }
        }

        #endregion

        #endregion
    }
}
