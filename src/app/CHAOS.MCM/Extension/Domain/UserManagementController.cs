namespace Chaos.Mcm.Extension.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;

    public class UserManagementController
    {
        public IMcmRepository McmRepository { get; set; }
        public string UsersFolderName { get; set; }
        public uint UserFolderTypeId { get; set; }
        public uint UserObjectTypeId { get; set; }

        public UserManagementController(IMcmRepository mcmRepository, string usersFolderName, uint userFolderTypeId, uint userObjectTypeId)
        {
            McmRepository = mcmRepository;
            UsersFolderName = usersFolderName;
            UserFolderTypeId = userFolderTypeId;
            UserObjectTypeId = userObjectTypeId;
        }

        #region GetFolderFromPath

        public Data.Dto.Standard.Folder GetFolderFromPath(bool failWhenMissing, string path)
        {
            return GetFolderFromPath(failWhenMissing, path.Split('/'));
        }

        public IList<Data.Dto.Object> GetUserObject(Guid userGuid, Guid requestingUsersGuid, bool createIfMissing = true, bool includeMetata = false, bool includeFiles = false)
        {
            var @object = McmRepository.ObjectGet(userGuid, includeMetata, includeFiles);

            if (@object != null)
                return new List<Data.Dto.Object> { @object };
            if (!createIfMissing)
                return new List<Data.Dto.Object>();

            var userFolder = GetUserFolder(userGuid, requestingUsersGuid).First();

            if (McmRepository.ObjectCreate(userGuid, UserObjectTypeId, userFolder.ID) != 1)
                throw new System.Exception("Failed to create user object");

            return new List<Data.Dto.Object> { McmRepository.ObjectGet(userGuid, includeMetata, includeFiles) };
        }

        public IList<Data.Dto.Standard.Folder> GetUserFolder(Guid userGuid, Guid requestingUsersGuid, bool createIfMissing = true)
        {
            var userFolder = GetFolderFromPath(false, UsersFolderName, userGuid.ToString());

            if (userFolder != null)
                return new List<Data.Dto.Standard.Folder> { userFolder };

            if (!createIfMissing)
                return new List<Data.Dto.Standard.Folder>();

            var usersFolder = GetFolderFromPath(false, UsersFolderName);

            var userFolderId = McmRepository.FolderCreate(requestingUsersGuid, null, userGuid.ToString(), usersFolder.ID, UserFolderTypeId);

            return McmRepository.FolderGet(userFolderId);
        }

        private Data.Dto.Standard.Folder GetFolderFromPath(bool failWhenMissing, params string[] path)
        {
            var folders = McmRepository.FolderGet();

            if (folders == null)
                throw new Exception("No folders found");

            var folder = GetFolderFromPath(null, path.ToList(), folders);

            if (failWhenMissing && folder == null)
                throw new Exception("Could not find folder: " + path.Aggregate((a, e) => a + "/" + e));

            return folder;
        }

        private static Data.Dto.Standard.Folder GetFolderFromPath(uint? parentId, IList<string> path, IList<Data.Dto.Standard.Folder> folders)
        {
            foreach (var folder in folders)
            {
                if (folder.ParentID == parentId && folder.Name == path[0])
                {
                    if (path.Count == 1)
                        return folder;

                    path.RemoveAt(0);
                    folders.Remove(folder);

                    return GetFolderFromPath(folder.ID, path, folders);
                }
            }

            return null;
        }

        #endregion
    }
}
