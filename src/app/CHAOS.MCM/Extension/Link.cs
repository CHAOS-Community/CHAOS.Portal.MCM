namespace Chaos.Mcm.Extension
{
    using System;

    using Chaos.Mcm.Permission;
    using Chaos.Portal;
    using Chaos.Portal.Data.Dto.Standard;
    using Chaos.Portal.Exceptions;
    using Chaos.Portal.Extension;

    public class Link : AMcmExtension
    {
        public ScalarResult Create(ICallContext callContext, Guid objectGuid, uint folderID)
        {
            if (!HasPermissionToObject(callContext, objectGuid, FolderPermission.CreateLink))
                    throw new InsufficientPermissionsException("User can only create links");

            // TODO: Manage magical number better (ObjectFolderTypeID:2 is link by default)
            var result = McmRepository.LinkCreate(objectGuid, folderID, 2);
                            
//          PutObjectInIndex( callContext.IndexManager.GetIndex<Mcm>(), db.Object_Get( objectGuid , true, true, true, true, true ).ToDto().ToList() );

            return new ScalarResult((int)result);
        }

        public ScalarResult Update(ICallContext callContext, Guid objectGuid, uint folderID, uint newFolderID)
        {
            if (!HasPermissionToObject(callContext, objectGuid, FolderPermission.CreateLink))
                throw new InsufficientPermissionsException("User does not have permission to update link");

//          PutObjectInIndex( callContext.IndexManager.GetIndex<Mcm>(), db.Object_Get( objectGuid , true, true, true, true, true ).ToDto().ToList() );

            var result = McmRepository.LinkUpdate(objectGuid, folderID, newFolderID);
            
            return new ScalarResult((int)result);
        }

        public ScalarResult Delete(ICallContext callContext, Guid objectGuid, uint folderID)
        {
            if (!HasPermissionToObject(callContext, objectGuid, FolderPermission.CreateLink))
                    throw new InsufficientPermissionsException("User does not have permission to delete link");

            var result = McmRepository.LinkDelete(objectGuid, folderID);

//                PutObjectInIndex( callContext.IndexManager.GetIndex<Mcm>(), db.Object_Get( objectGuid , true, true, true, true, true ).ToDto().ToList() );
          
            return new ScalarResult((int)result);
        } 
    }
}