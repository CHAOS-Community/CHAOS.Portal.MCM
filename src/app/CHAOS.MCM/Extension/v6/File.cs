using System;
using Chaos.Mcm.Data;
using Chaos.Mcm.Permission;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Data.Model;
using Chaos.Portal.Core.Exceptions;

namespace Chaos.Mcm.Extension.v6
{
    public class File : AMcmExtension
    {
        #region Initialization

        public File(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
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

            return result;
		}

		public ScalarResult Delete(uint id )
		{
		    var file = McmRepository.FileGet(id);
            
            if (!HasPermissionToObject(file.ObjectGuid, FolderPermission.CreateUpdateObjects))
                throw new InsufficientPermissionsException("User does not have permissions to delete a file on this object");

		    var result = McmRepository.FileDelete(id);

            return new ScalarResult((int)result);
        }

        #endregion
    }
}
