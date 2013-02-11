namespace Chaos.Mcm.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Chaos.Mcm.Data.Connection.MySql;
    using Chaos.Mcm.Data.Dto;
    using Chaos.Mcm.Data.Dto.Standard;
    using Chaos.Mcm.Exception;
    using Chaos.Mcm.Permission;
    using Chaos.Portal.Exceptions;

    using MySql.Data.MySqlClient;

    using Object = Chaos.Mcm.Data.Dto.Object;

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
            this._connectionString = connectionString;
            this.Gateway           = new Gateway(connectionString);
            return this;
        }

//        private MCMEntities CreateMcmEntities()
//        {
//            return new MCMEntities(this._connectionString);
//        }

        #endregion
        #region Business Logic

        #region Object Relation

        public uint ObjectRelationDelete(Guid object1Guid, Guid object2Guid, uint objectRelationTypeID)
        {
            var result = Gateway.ExecuteNonQuery("ObjectRelation_Delete", new[]
                {
                    new MySqlParameter("Object1Guid", object1Guid.ToByteArray()), 
                    new MySqlParameter("Object2Guid", object2Guid.ToByteArray()), 
                    new MySqlParameter("ObjectRelationTypeID", objectRelationTypeID )
                });

            return (uint)result;
        }

        public IList<ObjectRelationInfo> ObjectRelationInfoGet(Guid objectGuid)
        {
            return this.Gateway.ExecuteQuery<ObjectRelationInfo>("ObjectRelationInfo_Get", new MySqlParameter("Object1Guid", objectGuid.ToByteArray()));
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
        #region Metadata

        public IEnumerable<Metadata> MetadataGet(Guid guid)
        {
            return this.Gateway.ExecuteQuery<Metadata>("Metadata_Get", new MySqlParameter("Guid", guid.ToByteArray()));
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

            if (result == -200) throw new UnhandledException("Metadata set failed on the database and was rolled back");

            return (uint)result;
        }

        #endregion
        #region Object

        public uint ObjectDelete(Guid guid)
        {
            var result = Gateway.ExecuteNonQuery("Object_Delete", new[]
                         {
                             new MySqlParameter("Guid", guid.ToByteArray()) 
                         });

            if (result == -200)
                throw new UnhandledException("Object was not deleted, database rolled back");

            return (uint)result;
        }

        public uint ObjectCreate(Guid guid, uint objectTypeID, uint folderID)
        {
            var result = Gateway.ExecuteNonQuery("Object_Create", new[]
                         {
                             new MySqlParameter("Guid", guid.ToByteArray()),
                             new MySqlParameter("ObjectTypeID", objectTypeID),
                             new MySqlParameter("FolderID", folderID) 
                         });

            if (result == -200)
                throw new UnhandledException("Unhandled exception, Set was rolled back");


            return (uint)result;
        }

        public IList<Object> ObjectGet(uint? folderID = null, uint pageIndex = 0, uint pageSize = 5, bool includeMetadata = false, bool includeFiles = false, bool includeObjectRelations = false, bool includeFolders = false, bool includeAccessPoints = false)
        {
            return this.Gateway.ExecuteQuery<Object>("Object_GetByFolderID", new[]
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

        public Object ObjectGet(Guid objectGuid, bool includeMetadata = false, bool includeFiles = false, bool includeObjectRelations = false, bool includeFolders = false, bool includeAccessPoints = false)
        {
            return this.ObjectGet(new[] { objectGuid }, includeMetadata, includeFiles, includeObjectRelations, includeFolders, includeAccessPoints).FirstOrDefault();
        }

        public IList<Object> ObjectGet(IEnumerable<Guid> objectGuids, bool includeMetadata = false, bool includeFiles = false, bool includeObjectRelations = false, bool includeFolders = false, bool includeAccessPoints = false)
        {
            var guids = String.Join(",", objectGuids.Select(item => item.ToString().Replace("-", "")));

            return this.Gateway.ExecuteQuery<Object>("Object_GetByGUIDs", new[]
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

        public int FolderDelete(uint id)
        {
            var result = Gateway.ExecuteNonQuery("Folder_Delete", new MySqlParameter("ID", id));

            if (result == -200) throw new UnhandledException("An unknown error occured on folder_delete and was rolled back");
            if (result == -50) throw new FolderNotEmptyException("The folder has to be empty to be deleted");

            return (int)result;
        }

        public uint FolderCreate(Guid userGuid, Guid? subscriptionGuid, string name, uint? parentID, uint folderTypeID )
        {
            var result = Gateway.ExecuteNonQuery("Folder_Create", new[]
                {
                   new MySqlParameter("UserGuid", userGuid.ToByteArray()),  
                   new MySqlParameter("SubscriptionGuid", subscriptionGuid.HasValue ? subscriptionGuid.Value.ToByteArray() : null),  
                   new MySqlParameter("Name", name),  
                   new MySqlParameter("ParentID", parentID),  
                   new MySqlParameter("FolderTypeID", folderTypeID),  
                });

            if (result == -200) throw new UnhandledException("An unknown error occured on Folder_Create and was rolled back");

            if (result == -10) throw new UnhandledException("Invalid input parameters");

            return (uint)result;
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

            return (uint)result;
        }

        #endregion
        #region Format

        public IList<Format> FormatGet(uint? id = null, string name = null)
        {
            return Gateway.ExecuteQuery<Format>("Format_Get", new []
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

            return (uint)result;
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

            if (result == -100) throw new InsufficientPermissionsException("User does not have permission to delete an Object Type");

            return (uint)result;
        }

        public uint ObjectTypeSet(string name, uint? id = null)
        {
            var result = Gateway.ExecuteNonQuery("ObjectType_Set", new[]
                {
                    new MySqlParameter("ID", id), 
                    new MySqlParameter("Name", name) 
                });

            return (uint)result;
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

        public IList<MetadataSchema> MetadataSchemaGet(Guid userGuid, IEnumerable<Guid> groupGuids, Guid? metadataSchemaGuid, MetadataSchemaPermission permission )
        {
            var guids = String.Join(",", groupGuids.Select(item => item.ToString().Replace("-", "")));

            return Gateway.ExecuteQuery<MetadataSchema>("MetadataSchema_Get", new[]
                {
                    new MySqlParameter("UserGuid", userGuid.ToByteArray()), 
                    new MySqlParameter("GroupGuids", guids), 
                    new MySqlParameter("MetadataSchemaGuid", metadataSchemaGuid.HasValue ? metadataSchemaGuid.Value.ToByteArray() : null), 
                    new MySqlParameter("PermissionRequired", (int?)permission), 
                });
        }

        public uint MetadataSchemaSet(string name, XDocument schemaXml, Guid userGuid, Guid guid)
        {
            throw new NotImplementedException();
//            using (var db = DefaultMCMEntities)
//            {
//                var result = db.MetadataSchema_Set(guid.ToByteArray(), name, schemaXml, callContext.User.Guid.ToByteArray()).FirstOrDefault();
//
//                if (!result.HasValue || result.Value != 1)
//                    throw new UnhandledException("MetadataSchema was not created");
//
//                return result;
//            }
        }

        public uint MetadataSchemaDelete(Guid guid)
        {
            throw new NotImplementedException();
//            using (var db = DefaultMCMEntities)
//            {
//                var result = db.MetadataSchema_Delete(guid.ToByteArray()).FirstOrDefault();
//
//                if (result == null || !result.HasValue || result.Value != 1)
//                    throw new UnhandledException("MetadataSchema was not deleted");
//
//                return new ScalarResult(result.Value);
//            }
        }

        #endregion
        #region Folder User Join

        public IEnumerable<FolderUserJoin> GetFolderUserJoin()
        {
            throw new NotImplementedException();
//
//            using(var db = this.CreateMcmEntities())
//            {
//                return
//                    db.Folder_User_Join.ToList().Select(
//                        item =>
//                        new FolderUserJoin
//                            {
//                                FolderID = (uint)item.FolderID,
//                                UserGuid = item.UserGUID,
//                                Permission = (uint)item.Permission,
//                                DateCreated = item.DateCreated
//                            });
//            }
        }

        public uint SetFolderUserJoin(Guid userGuid, uint folderID, uint permission)
        {
            throw new NotImplementedException();
//
//            using(var db = this.CreateMcmEntities())
//            {
//                var result =
//                    db.Folder_User_Join_Set(userGuid.ToByteArray(), (int?)folderID, (int?)permission).FirstOrDefault();
//
//                if(!result.HasValue) throw new UnhandledException("Folder_User_Join_Set failed on the database and was rolled back");
//
//                return (uint)result.Value;
//            }
        }

        #endregion
        #region Folder

        public IEnumerable<FolderGroupJoin> GetFolderGroupJoin()
        {
            throw new NotImplementedException();

//            using(var db = this.CreateMcmEntities())
//            {
//                return
//                    db.Folder_Group_Join.ToList().Select(
//                        item =>
//                        new FolderGroupJoin
//                            {
//                                FolderID = (uint)item.FolderID,
//                                GroupGuid = item.GroupGUID,
//                                Permission = (uint)item.Permission,
//                                DateCreated = item.DateCreated
//                            });
//            }
        }

        public uint SetFolderGroupJoin(Guid groupGuid, uint folderID, uint permission)
        {
            throw new NotImplementedException();
//            using(var db = this.CreateMcmEntities())
//            {
//                var result =
//                    db.Folder_Group_Join_Set(groupGuid.ToByteArray(), (int?)folderID, (int?)permission).FirstOrDefault();
//
//                if(!result.HasValue) throw new UnhandledException("Folder_Group_Join_Set failed on the database and was rolled back");
//
//                return (uint)result.Value;
//            }
        }

        public IEnumerable<IFolderInfo> GetFolderInfo(IEnumerable<uint> ids)
        {
            throw new NotImplementedException();
//            var folderIDs = ids.Select(item => (long)item);
//            var folderIDStrings = string.Join(",", ids);
//
//            // TODO: optimize folder retrival form the database
//            using(var db = this.CreateMcmEntities())
//            {
//                return db.FolderInfo.Where(fi => folderIDs.Contains(fi.ID)).ToList().ToDto();
//            }
        }

        #endregion
        #region AccessPoint

        public IEnumerable<AccessPoint> GetAccessPoint(Guid accessPointGuid, Guid userGuid, IEnumerable<Guid> groupGuids, uint permission)
        {
            throw new NotImplementedException();
//            var groupGuidsString = string.Join(",", groupGuids);
//
//            using (var db = this.CreateMcmEntities())
//            {
//                return db.AccessPoint_Get(accessPointGuid.ToByteArray(), userGuid.ToByteArray(), groupGuidsString, (int?)permission).ToList().ToDto();
//            }
        }

        public uint SetAccessPointPublishSettings( Guid accessPointGuid, Guid objectGuid, DateTime? startDate, DateTime? endDate )
        {
            throw new NotImplementedException();
//            using (var db = this.CreateMcmEntities())
//            {
//                var result = db.AccessPoint_Object_Join_Set(accessPointGuid.ToByteArray(), objectGuid.ToByteArray(), startDate, endDate).FirstOrDefault();
//
//                if(!result.HasValue)
//                    throw new UnhandledException("SetAccessPointPublishSettings failed on the database, and was rolled back");
//
//                return (uint) result.Value;
//            }
        }

        #endregion
        #region Link

        public uint LinkCreate(Guid objectGuid, uint folderID, int objectFolderTypeID)
        {
            throw new NotImplementedException();
//            using (MCMEntities db = DefaultMCMEntities)
//            {
//                var result = db.Object_Folder_Join_Create(objectGuid.ToByteArray(), (int)folderID, 2).FirstOrDefault();
//
//                if (!result.HasValue)
//                    throw new UnhandledException("Link create failed on the database and was rolled back");
//
//                if (result.Value == -100)
//                    throw new InsufficientPermissionsException("User can only create links");
//
//                //                PutObjectInIndex( callContext.IndexManager.GetIndex<Mcm>(), db.Object_Get( objectGuid , true, true, true, true, true ).ToDto().ToList() );
//
//                return result;
//            }
        }

        public uint LinkUpdate(Guid objectGuid, uint folderID, uint newFolderID)
        {
            throw new NotImplementedException();
//            using (MCMEntities db = DefaultMCMEntities)
//            {
//                var result = db.Object_Folder_Join_Update(objectGuid.ToByteArray(), (int)folderID, (int)newFolderID).First().Value;
//
//                return (uint)result;
//            }
        }

        public uint LinkDelete(Guid objectGuid, uint folderID)
        {
            throw new NotImplementedException();
//            using (MCMEntities db = DefaultMCMEntities)
//            {
//                var result = db.Object_Folder_Join_Delete(objectGuid.ToByteArray(), (int)folderID).FirstOrDefault();
//
//                if (!result.HasValue)
//                    throw new UnhandledException("Link delete failed on the database and was rolled back");
//
//                return (uint)result;
//            }
        }

        #endregion
        #region Destination

        public IEnumerable<DestinationInfo> DestinationGet(uint id)
        {
            throw new NotImplementedException();
//            using (MCMEntities db = DefaultMCMEntities)
//            {
//                return db.DestinationInfo_Get((int?)id).ToDto().ToList();
//            }
        }

        #endregion
        #region File

        public uint FileDelete(uint id)
        {
            throw new NotImplementedException();
            //            using (var db = DefaultMCMEntities)
            //            {
            //                var result = db.File_Delete((int?)id).FirstOrDefault();
            //
            //                if (!result.HasValue)
            //                    throw new UnhandledException("File delete failed in the database and was rolled back");
            //
            //                return result.Value;
            //            }
        }

        public uint FileCreate(Guid objectGuid, uint? parentID, uint destinationID, string filename, string originalFilename, string folderPath, uint formatID)
        {
            throw new NotImplementedException();
//            using (var db = DefaultMCMEntities)
//            {
//                var result = db.File_Create(objectGUID.ToByteArray(), (int?)parentFileID, (int)formatID, (int)destinationID, filename, originalFilename, folderPath).FirstOrDefault();
//
//                if (!result.HasValue)
//                    throw new UnhandledException("The creating the file failed in the database and was rolled back");
//
//                return result;
//            }
        }

        public IList<File> FileGet(uint id)
        {
            throw new NotImplementedException();
//            using (var db = DefaultMCMEntities)
//            {
//                return db.File_Get(result.Value).First().ToDto();
//            }
        }

        #endregion

        #endregion
    }
}
