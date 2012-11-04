using System;
using System.Collections.Generic;
using System.Linq;

namespace CHAOS.MCM.Permission.InMemory
{
    public class InMemoryPermissionManager : IPermissionManager
    {
        #region Fields

        private readonly IDictionary<uint, IFolder> _folders = new Dictionary<uint,IFolder>();

        #endregion
        #region Properties



        #endregion
        #region Construction



        #endregion
        #region Business logic

        /// <summary>
        /// Saves the folder 
        /// </summary>
        /// <param name="folder"></param>
        public void AddFolder(IFolder folder)
        {
            if (!_folders.ContainsKey(folder.ID))
                _folders.Add(folder.ID, folder);
            else
                _folders[folder.ID] = folder;

            if(folder.ParentFolder == null)
                return;

            folder.ParentFolder.AddSubFolder(folder);

            InheritParentPermissions(folder);
        }

        /// <summary>
        /// Adds of Combines the current folders permissions with that of the parent
        /// </summary>
        /// <param name="folder"></param>
        private static void InheritParentPermissions(IFolder folder)
        {
            foreach (var userPermission in folder.ParentFolder.UserPermissions)
            {
                SetEntityPermission(folder.UserPermissions, userPermission.Key, userPermission.Value);
            }

            foreach (var groupPermission in folder.ParentFolder.GroupPermissions)
            {
                SetEntityPermission(folder.GroupPermissions, groupPermission.Key, groupPermission.Value);
            }
        }

        /// <summary>
        /// Triggers all sub folders to inherit their parents permissions
        /// </summary>
        /// <param name="folder"></param>
        private static void PropagatePermissionsToSubFolders(IFolder folder)
        {
            foreach( var subFolder in folder.GetSubFolders() )
            {
                InheritParentPermissions(subFolder);
            }
        }

        private static void SetEntityPermission(IDictionary<Guid, FolderPermission> entityPermissions, Guid entityGuid, FolderPermission permission)
        {
            if (entityPermissions.ContainsKey(entityGuid))
                entityPermissions[entityGuid] = permission | entityPermissions[entityGuid];
            else
                entityPermissions.Add(entityGuid, permission);
        }

        /// <summary>
        /// Adds user permissions to a folder. If the user already exists then the permissions are merged
        /// </summary>
        /// <param name="folderID"></param>
        /// <param name="userPermission"></param>
        /// <returns>The EntityPermission object that was added/updated</returns>
        public void AddUser(uint folderID, IEntityPermission userPermission)
        {
            var folder = GetFolder(folderID);

            SetEntityPermission(folder.UserPermissions, userPermission.Guid, userPermission.Permission);

            PropagatePermissionsToSubFolders(folder);
        }

        /// <summary>
        /// Adds group permissions to a folder. If the group already exists then the permissions are merged
        /// </summary>
        /// <param name="folderID"></param>
        /// <param name="groupPermission"></param>
        public void AddGroup(uint folderID, IEntityPermission groupPermission)
        {
            var folder = GetFolder(folderID);

            SetEntityPermission(folder.GroupPermissions, groupPermission.Guid, groupPermission.Permission);

            PropagatePermissionsToSubFolders(folder);
        }

        #region Queries

        /// <summary>
        /// Looks up the folder from the in memory dictionary and returns it
        /// </summary>
        /// <param name="id">the id of the folder to return</param>
        /// <returns></returns>
        public IFolder GetFolder(uint id)
        {
            return _folders[id];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public IEnumerable<IFolder> GetTopFolders(FolderPermission permission, Guid userGuid)
        {
            return GetTopFolders(_folders.Values.Where(folder => folder.ParentFolder == null), permission, userGuid);
        }

        private static IEnumerable<IFolder> GetTopFolders(IEnumerable<IFolder> folders, FolderPermission permission, Guid userGuid)
        {
            foreach (var folder in folders)
            {
                if(folder.UserPermissions.ContainsKey(userGuid) && (folder.UserPermissions[userGuid] & permission) == permission )
                {
                    yield return folder;
                    continue;
                }

                foreach (var subFolder in GetTopFolders(folder.GetSubFolders(), permission, userGuid))
                {
                        yield return subFolder;
                }
            }
        }

        #endregion

        #endregion
    }
}