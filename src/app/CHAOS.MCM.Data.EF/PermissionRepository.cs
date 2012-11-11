using System.Collections.Generic;
using CHAOS.MCM.Data.Dto;
using Chaos.Mcm.Data;

namespace CHAOS.MCM.Data.EF
{
    public class PermissionRepository : IPermissionRepository
    {
        #region Fields

        private readonly IMcmRepository _mcmRepository = new McmRepository();

        #endregion
        #region Business Logic

        public IEnumerable<IFolder> GetFolder()
        {
            return _mcmRepository.GetFolder();
        }

        public IEnumerable<IFolderUserJoin> GetFolderUserJoin()
        {
            return _mcmRepository.GetFolderUserJoin();
        }

        public IEnumerable<IFolderGroupJoin> GetFolderGroupJoin()
        {
            return _mcmRepository.GetFolderGroupJoin();
        }

        #endregion
    }
}
