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
            _mcmRepository = mcmRepository;
        }

        #endregion
        #region Business Logic

        public IEnumerable<IFolder> FolderGet()
        {
            return _mcmRepository.FolderGet();
        }

        public IEnumerable<FolderPermission> FolderPermissionGet()
        {
            return _mcmRepository.FolderPermissionGet();
        }

        #endregion
    }
}
