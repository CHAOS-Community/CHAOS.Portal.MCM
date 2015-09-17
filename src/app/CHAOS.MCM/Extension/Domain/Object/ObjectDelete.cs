namespace Chaos.Mcm.Extension.Domain.Object
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Permission;
    using Portal.Core.Exceptions;
    using Portal.Core.Indexing.View;

    public class ObjectDelete : ExtensionHelperBase, IObjectDelete
    {
        public ObjectDelete(IMcmRepository mcmRepository, IPermissionManager permissionManager, IViewManager viewManager) : base(mcmRepository, permissionManager, viewManager)
        {
        }

        public uint Delete(Guid guid, Guid userId, IEnumerable<Guid> groupIds)
        {
            var objToDel = McmRepository.ObjectGet(guid, includeFolders: true);
            var folders = objToDel.ObjectFolders.Select(folder => PermissionManager.GetFolders(folder.ID));

            if (!PermissionManager.DoesUserOrGroupHavePermissionToFolders(userId, groupIds, FolderPermission.DeleteObject, folders))
                throw new InsufficientPermissionsException("User does not have permissions to remove object");

            var result = McmRepository.ObjectDelete(guid);

            if (result == 1)
                ViewManager.Delete(string.Format("Id:{0}", guid));

            return result;
        }
    }

    public interface IObjectDelete
    {
        uint Delete(Guid guid, Guid userId, IEnumerable<Guid> groupIds);
    }
}
