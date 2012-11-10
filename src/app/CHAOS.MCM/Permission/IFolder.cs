using System;
using System.Collections.Generic;

namespace CHAOS.MCM.Permission
{
    public interface IFolder
    {
        uint ID { get; set; }
        string Name { get; set; }
        DateTime DateCreated { get; set; }

        IFolder ParentFolder { get; set; }
        IEnumerable<IFolder> GetSubFolders();
        IEnumerable<IFolder> GetAncestorFolders();
        IDictionary<Guid, FolderPermission> UserPermissions { get; }
        IDictionary<Guid, FolderPermission> GroupPermissions { get; } 

        void AddSubFolder(IFolder folder);

        /// <summary>
        /// Adds a user permission to a folder
        /// </summary>
        /// <param name="folderID"></param>
        /// <param name="userPermission"></param>
        /// <returns>The userPermission object that was added</returns>
        void AddUser(IEntityPermission userPermission);

        /// <summary>
        /// Adds a group permission to a folder
        /// </summary>
        /// <param name="folderID"></param>
        /// <param name="groupPermission"></param>
        void AddGroup(IEntityPermission groupPermission);

        /// <summary>
        /// Returns true if the user or groups have the requested permission to the folder
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="groupGuids"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        bool DoesUserOrGroupHavePermission(Guid userGuid, IEnumerable<Guid> groupGuids, FolderPermission permission);
    }
}