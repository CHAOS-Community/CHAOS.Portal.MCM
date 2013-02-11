namespace Chaos.Mcm.Data
{
    using System.Collections.Generic;

    using Chaos.Mcm.Data.Dto;

    public class PermissionRepository : IPermissionRepository
    {
        #region Fields

        private readonly IMcmRepository _mcmRepository;

        #endregion
        #region Constructors

        public PermissionRepository(IMcmRepository mcmRepository)
        {
            this._mcmRepository = mcmRepository;
        }

        #endregion
        #region Business Logic

        public IEnumerable<IFolder> GetFolder()
        {
            return this._mcmRepository.FolderGet();
        }

        public IEnumerable<IFolderUserJoin> GetFolderUserJoin()
        {
            return this._mcmRepository.GetFolderUserJoin();
        }

        public IEnumerable<IFolderGroupJoin> GetFolderGroupJoin()
        {
            return this._mcmRepository.GetFolderGroupJoin();
        }

        #endregion
    }
}
