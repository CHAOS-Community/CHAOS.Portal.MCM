using System.Collections.Generic;

namespace CHAOS.MCM.Data.EF
{
    public class PermissionRepository : IPermissionRepository
    {
        #region Fields

        private readonly IMcmRepository _mcmRepository = new McmRepository();

        #endregion
        #region Business Logic

        public IEnumerable<DTO.Folder> GetFolder()
        {
            return _mcmRepository.GetFolder();
        }

        public IEnumerable<DTO.FolderUserJoin> GetFolderUserJoin()
        {
            return _mcmRepository.GetFolderUserJoin();
        }

        public IEnumerable<DTO.FolderGroupJoin> GetFolderGroupJoin()
        {
            return _mcmRepository.GetFolderGroupJoin();
        }

        #endregion
    }
}
