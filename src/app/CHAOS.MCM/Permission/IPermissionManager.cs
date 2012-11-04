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
        IFolder GetFolder(uint id);

        /// <summary>
        /// Get the top most folders that meet the permission
        /// </summary>
        /// <param name="permission">the permission to match</param>
        /// <param name="userGuid">the Guid of the user to check for</param>
        /// <returns></returns>
        IEnumerable<IFolder> GetTopFolders(FolderPermission permission, Guid userGuid);

        /// <summary>
        /// Adds a user permission to a folder
        /// </summary>
        /// <param name="folderID"></param>
        /// <param name="userPermission"></param>
        /// <returns>The userPermission object that was added</returns>
        void AddUser(uint folderID, IEntityPermission userPermission);

        /// <summary>
        /// Adds a group permission to a folder
        /// </summary>
        /// <param name="folderID"></param>
        /// <param name="groupPermission"></param>
        void AddGroup(uint folderID, IEntityPermission groupPermission);
    }
}