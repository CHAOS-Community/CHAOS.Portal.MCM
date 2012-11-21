using System;
using System.Collections.Generic;
using System.Linq;
using CHAOS.MCM.Data.Dto;
using CHAOS.MCM.Data.Dto.Standard;
using CHAOS.Portal.Exception;
using Chaos.Mcm.Data;

namespace CHAOS.MCM.Data.EF
{
    public class McmRepository : IMcmRepository
    {
        #region Fields

        private string _connectionString;

        #endregion
        #region Properties



        #endregion
        #region Construction

        public IMcmRepository WithConfiguration(string connectionString)
        {
            _connectionString = connectionString;

            return this;
        }

        private MCMEntities CreateMcmEntities()
        {
            return new MCMEntities(_connectionString);
        }

        #endregion
        #region Business Logic

        #region Folder

        public uint DeleteFolder(uint id)
        {
            using (var db = CreateMcmEntities())
            {
                var result = db.Folder_Delete((int?) id).FirstOrDefault();

                if(result.HasValue && result.Value == -200)
                    throw new UnhandledException("An unknown error occured on folder_delete and was rolled back");

                if(result.HasValue && result.Value == -50)
                    throw new InsufficientPermissionsException("The folder has to be empty to be deleted");

                return (uint) result.Value;
            }
        }

        #endregion
        public IEnumerable<IFolderUserJoin> GetFolderUserJoin()
        {
            using (var db = CreateMcmEntities())
            {
                return db.Folder_User_Join.ToList().Select(item => new FolderUserJoin
                                                             {
                                                                 FolderID    = (uint) item.FolderID,
                                                                 UserGuid    = item.UserGUID,
                                                                 Permission  = (uint) item.Permission,
                                                                 DateCreated = item.DateCreated
                                                             });
            }
        }

        public uint SetFolderUserJoin(Guid userGuid, uint folderID, uint permission)
        {
            using (var db = CreateMcmEntities())
            {
                var result = db.Folder_User_Join_Set( userGuid.ToByteArray(), (int?)folderID, (int?)permission).FirstOrDefault();

                if (!result.HasValue)
                    throw new UnhandledException("Folder_User_Join_Set failed on the database and was rolled back");

                return (uint) result.Value;
            }
        }

        public IEnumerable<IFolderGroupJoin> GetFolderGroupJoin()
        {
            using (var db = CreateMcmEntities())
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
            using (var db = CreateMcmEntities())
            {
                var result = db.Folder_Group_Join_Set(groupGuid.ToByteArray(), (int?)folderID, (int?)permission).FirstOrDefault();
                
                if(!result.HasValue)
                    throw new UnhandledException("Folder_Group_Join_Set failed on the database and was rolled back");

                return (uint) result.Value;
            }
        }

        public IEnumerable<IFolder> GetFolder()
        {
            using (var db = CreateMcmEntities())
            {
                return db.Folder_Get(null, null).ToDTO().ToList();
            }
        }

        public IEnumerable<IFolderInfo> GetFolderInfo(IEnumerable<uint> ids)
        {
            var folderIDs = ids.Select(item => (long) item);
            var folderIDStrings = string.Join(",", ids);

            // TODO: optimize folder retrival form the database
            using (var db = CreateMcmEntities())
            {
                return db.FolderInfo.Where(fi => folderIDs.Contains(fi.ID)).ToDTO().ToList();
            }
        }

        #region AccessPoint

        public IEnumerable<IAccessPoint> GetAccessPoint(Guid accessPointGuid, Guid userGuid, IEnumerable<Guid> groupGuids, uint permission )
        {
            var groupGuidsString = string.Join(",", groupGuids);

            using (var db = CreateMcmEntities())
            {
                return db.AccessPoint_Get(accessPointGuid.ToByteArray(), userGuid.ToByteArray(), groupGuidsString, (int?)permission).ToDto();
            }
        }

        public uint SetAccessPointPublishSettings( Guid accessPointGuid, Guid objectGuid, DateTime? startDate, DateTime? endDate )
        {
            using (var db = CreateMcmEntities())
            {
                var result = db.AccessPoint_Object_Join_Set(accessPointGuid.ToByteArray(), objectGuid.ToByteArray(), startDate, endDate).FirstOrDefault();

                if(!result.HasValue)
                    throw new UnhandledException("SetAccessPointPublishSettings failed on the database, and was rolled back");

                return (uint) result.Value;
            }
        }

        #endregion
        #region Object

        public IEnumerable<IObject> GetObject(Guid objectGuid, bool includeMetadata, bool includeFiles, bool includeObjectRelations, bool includeFolders, bool includeAccessPoint)
        {
            using (var db = CreateMcmEntities())
            {
                return db.Object_Get(objectGuid, true, true, true, true, true).ToDTO();
            }
        }

        #endregion

        #endregion
    }
}
