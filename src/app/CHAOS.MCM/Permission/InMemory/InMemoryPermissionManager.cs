using System.Collections.Generic;

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
            AddFolder(folder,null);
        }

        public void AddFolder(IFolder folder, IFolder parentFolder)
        {
            folder.Parent = parentFolder;

            if (!_folders.ContainsKey(folder.ID))
                _folders.Add(folder.ID, folder);
            else
                _folders[folder.ID] = folder;

            // TODO: Add User to subfolders

        }

        public void AddFolder(IFolder folder, uint parentFolderID)
        {
            AddFolder(folder, GetFolder(parentFolderID));
        }

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
        /// Adds user permissions to a folder. If the user already exists then the permissions are merged
        /// </summary>
        /// <param name="folderID"></param>
        /// <param name="userPermission"></param>
        /// <returns>The EntityPermission object that was added/updated</returns>
        public IEntityPermission AddUser(uint folderID, IEntityPermission userPermission)
        {
            var folder = GetFolder(folderID);
           
            if(folder.UserPermissions.ContainsKey(userPermission.Guid))
                folder.UserPermissions[userPermission.Guid].CombinePermission(with: userPermission.Permission);
            else
                folder.UserPermissions.Add(userPermission.Guid,userPermission);

            // TODO: Add to subfolders

            return folder.UserPermissions[userPermission.Guid];
        }

        #endregion
    }
}