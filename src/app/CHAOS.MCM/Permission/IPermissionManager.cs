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
        /// Adds a user permission to a folder
        /// </summary>
        /// <param name="folderID"></param>
        /// <param name="userPermission"></param>
        /// <returns>The userPermission object that was added</returns>
        IEntityPermission AddUser(uint folderID, IEntityPermission userPermission);
    }
}