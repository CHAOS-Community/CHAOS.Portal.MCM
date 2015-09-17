using System;
using System.Collections.Generic;
using Chaos.Mcm.Data;

namespace Chaos.Mcm.Permission
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

        /// <summary>
        /// Initialize the permission manager with a synchnization specification
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="synchronizationSpecification"></param>
        /// <returns></returns>
        IPermissionManager WithSynchronization(IMcmRepository repository, ISynchronizationSpecification synchronizationSpecification);

        /// <summary>
        /// Returns true if the user or groups have the requested permission to the folders
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="groupGuids"></param>
        /// <param name="permission"></param>
        /// <param name="folders"></param>
        /// <returns></returns>
        bool DoesUserOrGroupHavePermissionToFolders(Guid userGuid, IEnumerable<Guid> groupGuids, FolderPermission permission, IEnumerable<IFolder> folders);

        bool HasPermissionToObject(Guid objectGuid, Guid userGuid, IEnumerable<Guid> groupGuids, FolderPermission permissions);
    }
}