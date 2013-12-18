namespace Chaos.Mcm.Extension.Domain.Object
{
    using System;
    using System.Collections.Generic;
    using Data;
    using Permission;
    using Portal.Core.Exceptions;
    using Portal.Core.Indexing.View;

    class ObjectCreator : IObjectCreator
    {
        private IMcmRepository McmRepository { get; set; }
        private IPermissionManager PermissionManager { get; set; }
        private IViewManager ViewManager { get; set; }

        public ObjectCreator(IMcmRepository mcmRepository, IPermissionManager permissionManager, IViewManager viewManager)
        {
            McmRepository = mcmRepository;
            PermissionManager = permissionManager;
            ViewManager = viewManager;
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

    public interface IObjectCreator
    {
        Data.Dto.Object Create(Guid? guid, uint objectTypeID, uint folderID, Guid userId, IEnumerable<Guid> groupIds);
    }
}