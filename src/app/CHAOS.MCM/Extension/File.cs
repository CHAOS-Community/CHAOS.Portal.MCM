using System.Linq;
using CHAOS;
using Chaos.Mcm.Data.EF;
using Chaos.Mcm.Permission;
using Chaos.Portal;
using Chaos.Portal.Data.Dto.Standard;
using Chaos.Portal.Exceptions;
using Chaos.Portal.Extension;

namespace Chaos.Mcm.Extension
{
    [PortalExtension(configurationName : "MCM")]
    public class File : AMcmExtension
    {
        #region Business Logic

        public Data.Dto.Standard.File Create(ICallContext callContext, UUID objectGUID, uint? parentFileID, uint formatID, uint destinationID, string filename, string originalFilename, string folderPath)
		{
            using( var db = DefaultMCMEntities )
            {
                if( !HasPermissionToObject( callContext, objectGUID, FolderPermission.CreateUpdateObjects) )
                    throw new InsufficientPermissionsException("User does not have permissions to create a file for this object");

		        var result = db.File_Create( objectGUID.ToByteArray(), (int?) parentFileID, (int) formatID, (int) destinationID, filename, originalFilename, folderPath ).FirstOrDefault();

                if(!result.HasValue)
                    throw new UnhandledException("The creating the file failed in the database and was rolled back");

		        return db.File_Get( result.Value ).First().ToDto();
            }
        }

		public ScalarResult Delete( ICallContext callContext, uint ID )
		{
            using( var db = DefaultMCMEntities )
            {
                var file = db.File_Get((int?) ID).First().ToDto();

                if( !HasPermissionToObject( callContext, file.ObjectGUID, FolderPermission.CreateUpdateObjects) )
                    throw new InsufficientPermissionsException("User does not have permissions to delete a file on this object");

                var result = db.File_Delete((int?) ID).FirstOrDefault();

                if(!result.HasValue)
                    throw new UnhandledException("File delete failed in the database and was rolled back");

		        return new ScalarResult( result.Value );
            }
        }

        #endregion
    }
}
