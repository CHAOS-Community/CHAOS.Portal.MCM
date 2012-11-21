using System;
using System.Collections.Generic;
using System.Linq;
using CHAOS.MCM.Data.Dto;

namespace CHAOS.MCM.Permission.InMemory
{
    public class Folder : IFolder
    {
        #region Properties

        public uint ID { get; set; }
        public uint? ParentID { get; set; }
        public uint FolderTypeID { get; set; }
        public UUID SubscriptionGUID { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }        
        public IList<IFolder> SubFolders { get; private set; }
        public IFolder ParentFolder { get; set; }

        public IDictionary<Guid, FolderPermission> UserPermissions { get; set; }

        public IDictionary<Guid, FolderPermission> GroupPermissions { get; set; }

        #endregion
        #region Construction

        public Folder()
        {
            UserPermissions  = new Dictionary<Guid, FolderPermission>();
            GroupPermissions = new Dictionary<Guid, FolderPermission>();
            SubFolders       = new List<IFolder>();
        }

        #endregion
        #region Business Logic
        #region Permission

        /// <summary>
        /// Adds user permissions to a folder. If the user already exists then the permissions are merged
        /// </summary>
        /// <param name="userPermission"></param>
        /// <returns>The EntityPermission object that was added/updated</returns>
        public void AddUser(IEntityPermission userPermission)
        {
            SetEntityPermission(UserPermissions, userPermission.Guid, userPermission.Permission);

            PropagatePermissionsToSubFolders(this);
        }

        /// <summary>
        /// Adds group permissions to a folder. If the group already exists then the permissions are merged
        /// </summary>
        /// <param name="groupPermission"></param>
        public void AddGroup(IEntityPermission groupPermission)
        {
            SetEntityPermission(GroupPermissions, groupPermission.Guid, groupPermission.Permission);

            PropagatePermissionsToSubFolders(this);
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
            foreach (var subFolder in folder.GetSubFolders())
            {
                InheritParentPermissions(subFolder);
                PropagatePermissionsToSubFolders(subFolder);
            }
        }

        private static void SetEntityPermission(IDictionary<Guid, FolderPermission> entityPermissions, Guid entityGuid, FolderPermission permission)
        {
            if (entityPermissions.ContainsKey(entityGuid))
                entityPermissions[entityGuid] = entityPermissions[entityGuid].Or(permission);
            else
                entityPermissions.Add(entityGuid, permission);
        }

        /// <summary>
        /// Return true if user or group has permission to folder
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="groupGuids"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        public bool DoesUserOrGroupHavePermission(Guid userGuid, IEnumerable<Guid> groupGuids, FolderPermission permission)
        {
            return UserHasPemissionToFolder(userGuid, permission) || GroupsHavePermissionToFolder(groupGuids, permission);
        }

        private bool UserHasPemissionToFolder(Guid userGuid, FolderPermission permission)
        {
            return UserPermissions.ContainsKey(userGuid) && (UserPermissions[userGuid] & permission) == permission;
        }

        private bool GroupsHavePermissionToFolder(IEnumerable<Guid> groupGuids, FolderPermission permission)
        {
            return groupGuids.Any(groupGuid => GroupPermissions.ContainsKey(groupGuid) && (GroupPermissions[groupGuid] & permission) == permission);
        }
        
        #endregion

        public IEnumerable<IFolder> GetAncestorFolders()
        {
            yield return this;

            foreach (var ancestorFolder in ParentFolder.GetAncestorFolders())
                yield return ancestorFolder;
        }

        public void AddSubFolder(IFolder folder)
        {
            SubFolders.Add(folder);

            InheritParentPermissions(folder);
        }

        public IEnumerable<IFolder> GetSubFolders()
        {
            return SubFolders;
        }

        #endregion
    }
}
