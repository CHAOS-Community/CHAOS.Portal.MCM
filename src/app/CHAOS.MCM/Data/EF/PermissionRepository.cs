using System.Collections.Generic;
using Chaos.Mcm.Data.Dto;

namespace Chaos.Mcm.Data.EF
{
    public class PermissionRepository : IPermissionRepository
    {
        #region Fields

        private readonly IMcmRepository _mcmRepository;

        #endregion
        #region Constructors

        public PermissionRepository(IMcmRepository mcmRepository)
        {
            _mcmRepository = mcmRepository;
        }

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
