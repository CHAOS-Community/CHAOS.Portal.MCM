using System;
using System.Collections.Generic;
using System.Linq;
using Chaos.Mcm.Data;

namespace Chaos.Mcm.Permission.InMemory
{
    public class InMemoryPermissionManager : IPermissionManager
    {
        #region Fields

        private IDictionary<uint, IFolder> _folders = new Dictionary<uint,IFolder>();

        private IMcmRepository McmRepository { get; set; }

        #endregion
        #region Initialization

        public InMemoryPermissionManager()
        {
            
        }

        public InMemoryPermissionManager(IMcmRepository mcmRepository)
        {
            McmRepository = mcmRepository;
        }

        #endregion
        #region Business logic

        #region Adding folders

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

            if (folder.ParentFolder != null)
                folder.ParentFolder.AddSubFolder(folder);
        }

        #endregion
        #region Synchronization

        public IPermissionManager WithSynchronization(IMcmRepository repository, ISynchronizationSpecification synchronizationSpecification)
        {
            McmRepository = repository;
            synchronizationSpecification.OnSynchronizationTrigger += Synchronize;

            return this;
        }

        private void Synchronize(object sender, EventArgs e)
        {
            var tmp = new InMemoryPermissionManager();
            
            // FolderGet has to return folders ordered by ID, otherwise the parent won't be 
            foreach (var folder in McmRepository.FolderGet())
            {
                var permissionFolder = new Folder
                                           {
                                               ID          = folder.ID,
                                               Name        = folder.Name,
                                               DateCreated = folder.DateCreated
                                           };

                if(folder.ParentID.HasValue)
                    permissionFolder.ParentFolder = tmp.GetFolders((uint) folder.ParentID);

                tmp.AddFolder(permissionFolder);
            }

            foreach (var permission in McmRepository.FolderPermissionGet())
            {
                foreach(var entityPermission in permission.UserPermissions)
                    tmp.GetFolders(permission.FolderID).AddUser(entityPermission);

                foreach (var entityPermission in permission.GroupPermissions)
                    tmp.GetFolders(permission.FolderID).AddGroup(entityPermission);
            }

            _folders = tmp._folders;
        }

        #endregion
        #region Queries

        /// <summary>
        /// Looks up the folder from the in memory dictionary and returns it
        /// </summary>
        /// <param name="id">the id of the folder to return</param>
        /// <returns></returns>
        public IFolder GetFolders(uint id)
        {
            return _folders[id];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="userGuid"></param>
        /// <param name="groupGuids"></param>
        /// <returns></returns>
        public IEnumerable<IFolder> GetFolders(FolderPermission permission, Guid userGuid, IEnumerable<Guid> groupGuids)
        {
            return GetTopFolders(_folders.Values.Where(folder => folder.ParentFolder == null), permission, userGuid, groupGuids);
        }

        private static IEnumerable<IFolder> GetTopFolders(IEnumerable<IFolder> folders, FolderPermission permission, Guid userGuid, IEnumerable<Guid> groupGuids)
        {
            foreach (var folder in folders)
            {
                if ( folder.DoesUserOrGroupHavePermission(userGuid, groupGuids, permission) )
                    yield return folder;
                else
                    foreach (var subFolder in GetTopFolders(folder.GetSubFolders(), permission, userGuid, groupGuids))
                        yield return subFolder;
            }
        }

        /// <summary>
        /// Returns true if the user or groups have the requested permission to the folders
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="groupGuids"></param>
        /// <param name="permission"></param>
        /// <param name="folders"></param>
        /// <returns></returns>
        public bool DoesUserOrGroupHavePermissionToFolders(Guid userGuid, IEnumerable<Guid> groupGuids, FolderPermission permission, IEnumerable<IFolder> folders)
        {
            return folders.Any(f => f.DoesUserOrGroupHavePermission(userGuid, groupGuids, permission));
        }

        public bool HasPermissionToObject(Guid objectGuid, Guid userGuid, IEnumerable<Guid> groupGuids, FolderPermission permissions)
        {
            var folderGet = McmRepository.FolderGet(null, null, objectGuid: objectGuid);
            var folders = folderGet.Select(item => GetFolders(item.ID));

            return DoesUserOrGroupHavePermissionToFolders(userGuid, groupGuids, permissions, folders);
        }

        #endregion

        #endregion
    }
}