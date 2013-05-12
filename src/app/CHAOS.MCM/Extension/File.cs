namespace Chaos.Mcm.Extension
{
    using System;

    using Chaos.Mcm.Data;
    using Chaos.Mcm.Permission;
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Exceptions;

    public class File : AMcmExtension
    {
        #region Initialization

        public File(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
        {
        }

        public File(IPortalApplication portalApplication)
            : base(portalApplication)
        {
        }

        #endregion
        #region Business Logic

        public Data.Dto.File Create(Guid objectGuid, uint? parentFileID, uint formatID, uint destinationID, string filename, string originalFilename, string folderPath)
		{
            if (!HasPermissionToObject(objectGuid, FolderPermission.CreateUpdateObjects))
                throw new InsufficientPermissionsException("User does not have permissions to create a file for this object");

            var id     = McmRepository.FileCreate(objectGuid, parentFileID, destinationID, filename, originalFilename, folderPath, formatID);
            var result = McmRepository.FileGet(id);

            if(result.Count == 0) throw new UnhandledException("File was created but couldn't be retrieved, try to Call Get specifically");

            return result[0];
		}

		public ScalarResult Delete(uint id )
		{
		    var file = McmRepository.FileGet(id)[0];
            
            if (!HasPermissionToObject(file.ObjectGuid, FolderPermission.CreateUpdateObjects))
                throw new InsufficientPermissionsException("User does not have permissions to delete a file on this object");

		    var result = McmRepository.FileDelete(id);

            return new ScalarResult((int)result);
        }

        #endregion
    }
}
