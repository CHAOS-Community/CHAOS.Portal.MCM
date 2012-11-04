using System;
using System.Collections.Generic;

namespace CHAOS.MCM.Permission
{
    public interface IPermissionManager
    {
        /// <summary>
        /// Insert folder in permission manager
        /// </summary>
        /// <param name="folder"></param>
        void AddFolder(IFolder folder);

        /// <summary>
        /// Retrieve a folder from the permission manager
        /// </summary>
        /// <param name="id">the id of the folder</param>
        /// <returns></returns>
        IFolder GetFolders(uint id);

        /// <summary>
        /// Get the top most folders that meet the permission
        /// </summary>
        /// <param name="permission">the permission to match</param>
        /// <param name="userGuid">the Guid of the user to check for</param>
        /// <param name="groupGuids"> </param>
        /// <returns></returns>
        IEnumerable<IFolder> GetFolders(FolderPermission permission, Guid userGuid, IEnumerable<Guid> groupGuids);
    }
}