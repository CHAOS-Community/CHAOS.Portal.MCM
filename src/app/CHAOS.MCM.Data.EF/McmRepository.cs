using System.Collections.Generic;
using System.Linq;
using CHAOS.MCM.Data.Dto;
using CHAOS.MCM.Data.Dto.Standard;
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

        public IEnumerable<IFolderUserJoin> GetFolderUserJoin()
        {
            using (var db = CreateMcmEntities())
            {
                return db.Folder_User_Join.Select(item => new FolderUserJoin
                                                             {
                                                                 FolderID    = (uint) item.FolderID,
                                                                 UserGuid    = item.UserGUID,
                                                                 Permission  = (uint) item.Permission,
                                                                 DateCreated = item.DateCreated
                                                             });
            }
        }

        public IEnumerable<IFolderGroupJoin> GetFolderGroupJoin()
        {
            using (var db = CreateMcmEntities())
            {
                return db.Folder_Group_Join.Select(item => new FolderGroupJoin
                                                               {
                                                                   FolderID    = (uint) item.FolderID,
                                                                   GroupGuid   = item.GroupGUID,
                                                                   Permission  = (uint) item.Permission,
                                                                   DateCreated = item.DateCreated
                                                               });
            }
        }

        public IEnumerable<IFolder> GetFolder()
        {
            using (var db = CreateMcmEntities())
            {
                return db.Folder_Get(null, null).ToDTO();
            }
        }

        public IEnumerable<IFolderInfo> GetFolderInfo(IEnumerable<uint> ids)
        {
            var folderIDs = ids.Select(item => (long) item);

            // TODO: optimize folder retrival form the database
            using (var db = CreateMcmEntities())
            {
                return db.FolderInfo.Where(fi => folderIDs.Contains(fi.ID)).ToDTO().ToList();
            }
        }

        #endregion
    }
}
