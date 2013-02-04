namespace Chaos.Mcm.Data.Connection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CHAOS.Extensions;

    using Chaos.Mcm.Data.Connection.MySql;
    using Chaos.Mcm.Data.Dto;
    using Chaos.Mcm.Data.Dto.Standard;
    using Chaos.Mcm.Data.EF;
    using Chaos.Mcm.Exception;
    using Chaos.Mcm.Permission;
    using Chaos.Portal.Exceptions;

    using Metadata = Chaos.Mcm.Data.Dto.Standard.Metadata;

    public class McmRepository : IMcmRepository
    {
        #region Fields

        private string _connectionString;

        private Gateway _gateway;

        #endregion
        #region Properties



        #endregion
        #region Construction

        public IMcmRepository WithConfiguration(string connectionString)
        {
            _connectionString = connectionString;
            _gateway          = new Gateway(connectionString);
            return this;
        }

        private MCMEntities CreateMcmEntities()
        {
            return new MCMEntities(this._connectionString);
        }

        #endregion
        #region Business Logic

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

        #endregion
        #region Metadata Schema

        public IEnumerable<Dto.Standard.MetadataSchema> GetMetadataSchema(Guid userGuid, IEnumerable<Guid> groupGuids, Guid? metadataSchemaGuid, MetadataSchemaPermission permission )
        {
            using( var db = this.CreateMcmEntities() )
            {
                var sGroupGuids = string.Join(",", groupGuids.Select(guid => guid.ToUUID().ToString().Replace("-", "")));

                return db.MetadataSchema_Get(userGuid.ToByteArray(), sGroupGuids, metadataSchemaGuid.HasValue ? metadataSchemaGuid.Value.ToByteArray() : null, (int?)permission).ToList().ToDto();
			}
        }

        //public IEnumerable<Dto.Standard.MetadataSchemaInfo> GetMetadataSchemaInfo(Guid userGuid, IEnumerable<Guid> groupGuids, Guid? metadataSchemaGuid, MetadataSchemaPermission permission)
        //{
        //    using (var db = CreateMcmEntities())
        //    {
        //        var sGroupGuids = string.Join(",", groupGuids.Select(guid => guid.ToString().Replace("-", "")));

        //        return db.MetadataSchema_Get(userGuid.ToByteArray(), sGroupGuids, metadataSchemaGuid.HasValue ? metadataSchemaGuid.Value.ToByteArray() : null, (int?)permission).ToList().ToDTO();
        //    }
        //}

        #endregion
        #region Object Relation
    
        public uint ObjectRelationSet(Guid object1Guid, Guid object2Guid, uint objectRelationTypeID, int? sequence)
        {
            var result = _gateway.ObjectRelationSet(new ObjectRelation
            {
                Object1Guid = object1Guid,
                Object2Guid = object2Guid,
                ObjectRelationTypeID = objectRelationTypeID,
                Sequence = sequence
            });

            if (result == -100)
                throw new InsufficientPermissionsException("The user do not have permission to create object relations");

            if (result == -200)
                throw new ObjectRelationAlreadyExistException("The object relation already exists");

            return (uint)result;
        }

        public uint ObjectRelationSet(ObjectRelationInfo objectRelationInfo, Guid editingUserGuid)
        {
            var result = _gateway.ObjectRelationSetMetadata(objectRelationInfo, editingUserGuid);

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


        public IEnumerable<Dto.Standard.FolderGroupJoin> GetFolderGroupJoin()
        {
            using (var db = this.CreateMcmEntities())
            {
                return db.Folder_Group_Join.ToList().Select(item => new FolderGroupJoin
                                                               {
                                                                   FolderID    = (uint) item.FolderID,
                                                                   GroupGuid   = item.GroupGUID,
                                                                   Permission  = (uint) item.Permission,
                                                                   DateCreated = item.DateCreated
                                                               });
            }
        }

        public uint SetFolderGroupJoin(Guid groupGuid, uint folderID, uint permission)
        {
            using (var db = this.CreateMcmEntities())
            {
                var result = db.Folder_Group_Join_Set(groupGuid.ToByteArray(), (int?)folderID, (int?)permission).FirstOrDefault();
                
                if(!result.HasValue)
                    throw new UnhandledException("Folder_Group_Join_Set failed on the database and was rolled back");

                return (uint) result.Value;
            }
        }

        public IEnumerable<Dto.Standard.Folder> GetFolder()
        {
            using (var db = this.CreateMcmEntities())
            {
                return db.Folder_Get(null, null).ToList().ToDto();
            }
        }

        public IEnumerable<IFolderInfo> GetFolderInfo(IEnumerable<uint> ids)
        {
            var folderIDs = ids.Select(item => (long) item);
            var folderIDStrings = string.Join(",", ids);

            // TODO: optimize folder retrival form the database
            using (var db = this.CreateMcmEntities())
            {
                return db.FolderInfo.Where( fi => folderIDs.Contains( fi.ID ) ).ToList().ToDto();
            }
        }

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

        public IEnumerable<Dto.Standard.Object> GetObject(Guid objectGuid, bool includeMetadata, bool includeFiles, bool includeObjectRelations, bool includeFolders, bool includeAccessPoint)
        {
            using (var db = this.CreateMcmEntities())
            {
                return db.Object_Get(objectGuid, true, true, true, true, true).ToDto();
            }
        }

        public IEnumerable<Dto.Standard.Object> GetObject(IEnumerable<Guid> objectGuids, bool includeMetadata, bool includeFiles, bool includeObjectRelations, bool includeFolders, bool includeAccessPoint)
        {
            using (var db = this.CreateMcmEntities())
            {
                return db.Object_Get(objectGuids, true, true, true, true, true).ToDto();
            }
        }

        public IEnumerable<Dto.Standard.Object> GetObject(Guid relatedToObjectWithGuid, uint? objectRelationTypeID)
        {
            using (var db = this.CreateMcmEntities())
            {
                return db.Object_Get(relatedToObjectWithGuid, objectRelationTypeID, true, true, true, true).ToList().ToDto();
            }
        }

        #endregion

        #endregion
    }
}
