namespace Chaos.Mcm.Extension
{
    using System;

    using Chaos.Mcm.Data;
    using Chaos.Mcm.Permission;
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Exceptions;

    public class Link : AMcmExtension
    {
        #region Initialization

        public Link(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
        {
        }

        public Link(IPortalApplication portalApplication)
            : base(portalApplication)
        {
        }

        #endregion

        //todo: re-add indexing to links
        public ScalarResult Create(Guid objectGuid, uint folderID)
        {
            if (!HasPermissionToObject(objectGuid, FolderPermission.CreateLink)) throw new InsufficientPermissionsException("User can only create links");

            // TODO: Manage magical number better (ObjectFolderTypeID:2 is link by default)
            var result = McmRepository.LinkCreate(objectGuid, folderID, 2);
                            
//          PutObjectInIndex( callContext.IndexManager.GetIndex<Mcm>(), db.Object_Get( objectGuid , true, true, true, true, true ).ToDto().ToList() );

            return new ScalarResult((int)result);
        }

        public ScalarResult Update(Guid objectGuid, uint folderID, uint newFolderID)
        {
            if (!HasPermissionToObject(objectGuid, FolderPermission.CreateLink)) throw new InsufficientPermissionsException("User does not have permission to update link");

//          PutObjectInIndex( callContext.IndexManager.GetIndex<Mcm>(), db.Object_Get( objectGuid , true, true, true, true, true ).ToDto().ToList() );

            var result = McmRepository.LinkUpdate(objectGuid, folderID, newFolderID);
            
            return new ScalarResult((int)result);
        }

        public ScalarResult Delete(Guid objectGuid, uint folderID)
        {
            if (!HasPermissionToObject(objectGuid, FolderPermission.CreateLink)) throw new InsufficientPermissionsException("User does not have permission to delete link");

            var result = McmRepository.LinkDelete(objectGuid, folderID);

//                PutObjectInIndex( callContext.IndexManager.GetIndex<Mcm>(), db.Object_Get( objectGuid , true, true, true, true, true ).ToDto().ToList() );
          
            return new ScalarResult((int)result);
        } 
    }
}