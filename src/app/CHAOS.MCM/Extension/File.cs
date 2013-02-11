namespace Chaos.Mcm.Extension
{
    using System;

    using Chaos.Mcm.Permission;
    using Chaos.Portal;
    using Chaos.Portal.Data.Dto.Standard;
    using Chaos.Portal.Exceptions;

    public class File : AMcmExtension
    {
        #region Business Logic

        public Data.Dto.Standard.File Create(ICallContext callContext, Guid objectGuid, uint? parentFileID, uint formatID, uint destinationID, string filename, string originalFilename, string folderPath)
		{
            if (!HasPermissionToObject(callContext, objectGuid, FolderPermission.CreateUpdateObjects))
                throw new InsufficientPermissionsException("User does not have permissions to create a file for this object");

            var id     = McmRepository.FileCreate(objectGuid, parentFileID, destinationID, filename, originalFilename, folderPath, formatID);
            var result = McmRepository.FileGet(id);

            if(result.Count == 0)
                throw new UnhandledException("File was created but couldn't be retrieved, try to Call Get specifically");

            return result[0];
		}

		public ScalarResult Delete( ICallContext callContext, uint id )
		{
		    var file = McmRepository.FileGet(id)[0];
            
            if (!HasPermissionToObject(callContext, file.ObjectGuid, FolderPermission.CreateUpdateObjects))
                throw new InsufficientPermissionsException("User does not have permissions to delete a file on this object");

		    var result = McmRepository.FileDelete(id);

            return new ScalarResult((int)result);
        }

        #endregion
    }
}
