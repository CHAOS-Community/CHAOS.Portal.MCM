namespace CHAOS.MCM.Module
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;

    using Amazon.S3;

    using CHAOS.MCM.Data.EF;
    using CHAOS.MCM.Permission;
    using CHAOS.Portal.Core;
    using CHAOS.Portal.Core.Module;
    using CHAOS.Portal.DTO.Standard;
    using CHAOS.Portal.Exception;

    using FileInfo = CHAOS.MCM.Data.Dto.Standard.FileInfo;

    [Module("MCM")]
    public class DownloadModule : AMCMModule
    {
        #region Business Logic

        [Datatype("Download","Get")]
        public Attachment Get(ICallContext callContext, uint fileId)
        {
        //    if(callContext.IsAnonymousUser) throw new InsufficientPermissionsException("User has to be logged in");

            var file = GetFile(fileId);

            if(file == null) throw new ConfigurationException("Requested file does not have a properly configured access provider associated");

            var args       = file.URL.Split(';');
            var bucketname = args[0].Substring(11);
            var key        = args[1].Substring(4).TrimStart('/');

         //   if (!HasPermissionToObject(callContext, file.ObjectGUID, FolderPermission.Read)) throw new InsufficientPermissionsException("Read permission is required");

            if (!callContext.PortalRequest.Parameters.ContainsKey("format"))
                callContext.PortalRequest.Parameters.Add("format", ReturnFormat.ATTACHMENT.ToString());
            else
                callContext.PortalRequest.Parameters["format"] = ReturnFormat.ATTACHMENT.ToString();

            using (var client = new AmazonS3Client(AccessKey, SecretKey))
            {
                try
                {
                    var request = new Amazon.S3.Model.GetObjectRequest().WithBucketName(bucketname)
                                                                        .WithKey(key);

                    var response = client.GetObject(request);

                    return new Attachment
                        {
                            FileName    = file.OriginalFilename,
                            ContentType = file.MimeType,
                            Disposable  = response,
                            Stream      = response.ResponseStream
                        };
                }
                catch(Amazon.S3.AmazonS3Exception e)
                {
                    throw new Exception(string.Format("bucket: {0}, key: {1}", bucketname, key), e);
                }
            }
        }

        private FileInfo GetFile(uint id)
        {
            using (var db = DefaultMCMEntities)
            {
                var file = db.File_Get((int?)id).First().ToDTO();
                var obj  = db.Object_Get(file.ObjectGUID, false, true, false, false, false).First().ToDTO();
                
                return obj.Files.FirstOrDefault(item => item.ID == id && item.Token == "S3");
            }
        }

        #endregion
    }
}
