using System.Linq;
using CHAOS.MCM.Data.EF;
using CHAOS.Portal.Core;
using CHAOS.Portal.Core.Module;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Portal.Exception;

namespace CHAOS.MCM.Module
{
    using CHAOS.MCM.Data.Dto.Standard;

    using FolderPermission = CHAOS.MCM.Permission.FolderPermission;

    [Module("MCM")]
    public class FileModule : AMCMModule
    {
        #region Business Logic

		[Datatype("File", "Create")]
        public Data.Dto.Standard.File Create(ICallContext callContext, UUID objectGUID, uint? parentFileID, uint formatID, uint destinationID, string filename, string originalFilename, string folderPath)
		{
            callContext.Log.Debug(callContext.Session.GUID.ToString());
            callContext.Log.Debug(callContext.User.GUID.ToString());
            callContext.Log.Debug(objectGUID.ToString());
            callContext.Log.Debug(formatID.ToString());
            callContext.Log.Debug(destinationID.ToString());
            callContext.Log.Debug(filename);
            callContext.Log.Debug(originalFilename);
            callContext.Log.Debug(folderPath);

            using( var db = DefaultMCMEntities )
            {
                if( !HasPermissionToObject( callContext, objectGUID, FolderPermission.CreateUpdateObjects) )
                    throw new InsufficientPermissionsException("User does not have permissions to create a file for this object");

		        var result = db.File_Create( objectGUID.ToByteArray(), (int?) parentFileID, (int) formatID, (int) destinationID, filename, originalFilename, folderPath ).FirstOrDefault();

                if(!result.HasValue)
                    throw new UnhandledException("The creating the file failed in the database and was rolled back");

		        return db.File_Get( result.Value ).First().ToDTO();
            }
        }

        [Datatype("File", "Delete")]
		public ScalarResult Delete( ICallContext callContext, uint ID )
		{
            using( var db = DefaultMCMEntities )
            {
                var file = db.File_Get((int?) ID).First().ToDTO();

                if( !HasPermissionToObject( callContext, file.ObjectGUID, FolderPermission.CreateUpdateObjects) )
                    throw new InsufficientPermissionsException("User does not have permissions to delete a file on this object");

                var fileInfo = GetFile(ID);

                if (fileInfo != null) RemoveFile(fileInfo);

                var result = db.File_Delete((int?) ID).FirstOrDefault();

                if(!result.HasValue) throw new UnhandledException("File delete failed in the database and was rolled back");

		        return new ScalarResult( result.Value );
            }
        }

        private FileInfo GetFile(uint id)
        {
            using (var db = DefaultMCMEntities)
            {
                var file = db.File_Get((int?)id).First().ToDTO();
                var obj = db.Object_Get(file.ObjectGUID, false, true, false, false, false).First().ToDTO();

                return obj.Files.FirstOrDefault(item => item.ID == id && item.Token == "S3");
            }
        }

        #endregion
    }
}
