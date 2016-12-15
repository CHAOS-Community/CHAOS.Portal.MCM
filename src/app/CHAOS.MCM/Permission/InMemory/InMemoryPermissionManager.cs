using System;
using System.Collections.Generic;
using System.Linq;
using CHAOS.MCM.Data;
using CHAOS.MCM.Data.Dto.Standard;

namespace CHAOS.MCM.Permission.InMemory
{
    public class InMemoryPermissionManager : IPermissionManager
    {
        #region Fields

        private IDictionary<uint, IFolder> _folders = new Dictionary<uint,IFolder>();
        private IPermissionRepository _permissionRepository;

        #endregion
        #region Properties



        #endregion
        #region Construction



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

        public IPermissionManager WithSynchronization(IPermissionRepository repository, ISynchronizationSpecification synchronizationSpecification)
        {
            _permissionRepository = repository;
            synchronizationSpecification.OnSynchronizationTrigger += Synchronize;

            return this;
        }

        private void Synchronize(object sender, EventArgs e)
        {
	        try
	        {
				var tmp = new InMemoryPermissionManager();

				// GetFolder has to return folders ordered by ID, otherwise the parent won't be 
				foreach (var folder in _permissionRepository.GetFolder())
				{
					var permissionFolder = new Folder
					{
						ID = folder.ID,
						Name = folder.Name,
						DateCreated = folder.DateCreated
					};

					if (folder.ParentID.HasValue)
						permissionFolder.ParentFolder = tmp.GetFolders((uint)folder.ParentID);

					tmp.AddFolder(permissionFolder);
				}

				foreach (var folderUserJoin in _permissionRepository.GetFolderUserJoin())
				{
					tmp.GetFolders(folderUserJoin.FolderID).AddUser(new EntityPermission
					{
						Guid = folderUserJoin.UserGuid,
						Permission = (FolderPermission)folderUserJoin.Permission
					});
				}

				foreach (var folderGroupJoin in _permissionRepository.GetFolderGroupJoin())
				{
					tmp.GetFolders(folderGroupJoin.FolderID).AddGroup(new EntityPermission
					{
						Guid = folderGroupJoin.GroupGuid,
						Permission = (FolderPermission)folderGroupJoin.Permission
					});
				}

				_folders = tmp._folders;
			}
	        catch
	        {
	        }
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

        #endregion

        #endregion
    }
}