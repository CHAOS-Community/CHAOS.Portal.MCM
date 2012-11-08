using System.Collections.Generic;
using System.Linq;
using CHAOS.MCM.Data.DTO;

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

        public IEnumerable<FolderUserJoin> GetFolderUserJoin()
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

        public IEnumerable<FolderGroupJoin> GetFolderGroupJoin()
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

        public IEnumerable<DTO.Folder> GetFolder()
        {
            using (var db = CreateMcmEntities())
            {
                return db.Folder_Get(null, null).ToDTO();
            }
        }

        #endregion
    }
}
