namespace Chaos.Mcm.Extension.Domain.Object
{
    using System;
    using System.Collections.Generic;
    using Data;
    using Permission;
    using Portal.Core.Exceptions;
    using Portal.Core.Indexing.View;

    public class ObjectCreate : ExtensionHelperBase, IObjectCreate
    {
        public ObjectCreate(IMcmRepository mcmRepository, IPermissionManager permissionManager, IViewManager viewManager) : base(mcmRepository, permissionManager, viewManager)
        {
        }

        public Data.Dto.Object Create(Guid? guid, uint objectTypeID, uint folderID, Guid userId, IEnumerable<Guid> groupIds)
        {
            if (!PermissionManager.GetFolders(folderID).DoesUserOrGroupHavePermission(userId, groupIds, FolderPermission.CreateUpdateObjects))
                throw new InsufficientPermissionsException("User does not have permissions to create object");

            guid = guid.HasValue ? guid : Guid.NewGuid();

            McmRepository.ObjectCreate(guid.Value, objectTypeID, folderID);

            var result = McmRepository.ObjectGet(guid.Value);

            ViewManager.Index(result);

            return result;
        } 
    }

    public interface IObjectCreate
    {
        Data.Dto.Object Create(Guid? guid, uint objectTypeID, uint folderID, Guid userId, IEnumerable<Guid> groupIds);
    }
}