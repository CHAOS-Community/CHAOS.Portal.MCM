using System.Linq;
using CHAOS.MCM.Data.EF;
using CHAOS.MCM.Module.Rights;
using CHAOS.Portal.Core;
using CHAOS.Portal.Core.Module;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Portal.Exception;

namespace CHAOS.MCM.Module
{
    [Module("MCM")]
    public class FileModule : AMCMModule
    {
        #region Business Logic

		[Datatype("File", "Create")]
		public Data.DTO.File Create( ICallContext callContext, UUID objectGUID, uint? parentFileID, uint formatID, uint destinationID, string filename, string originalFilename, string folderPath )
		{
            using( var db = DefaultMCMEntities )
            {
                if( !HasPermissionToObject( callContext, objectGUID, FolderPermissions.CreateUpdateObjects) )
                    throw new InsufficientPermissionsException("User does not have permissions to create a file for this object");

		        var id = db.File_Create( objectGUID.ToByteArray(), (int?) parentFileID, (int) formatID, (int) destinationID, filename, originalFilename, folderPath ).First().Value;

		        return db.File_Get( id ).First().ToDTO();
            }
        }

        [Datatype("File", "Delete")]
		public ScalarResult Delete( ICallContext callContext, uint ID )
		{
            using( var db = DefaultMCMEntities )
            {
                var file = db.File_Get((int?) ID).First().ToDTO();

                if( !HasPermissionToObject( callContext, file.ObjectGUID, FolderPermissions.CreateUpdateObjects) )
                    throw new InsufficientPermissionsException("User does not have permissions to delete a file on this object");

		        return new ScalarResult( db.File_Delete( (int?) ID ).First().Value );
            }
        }

        #endregion
    }
}
