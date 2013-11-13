namespace Chaos.Mcm.Extension.v5
{
    using System.Linq;
    using Data;
    using Data.Dto;
    using Portal.Core;
    using Portal.Core.Data.Model;
    using Portal.Core.Exceptions;
    using Portal.Core.Extension.v5;

    public class Download : AExtension
    {
        public IMcmRepository McmRepository { get; set; }

        public Download(IPortalApplication portalApplication, IMcmRepository mcmRepository, string moduleName): base(portalApplication, moduleName)
        {
            McmRepository = mcmRepository;
        }


        public Attachment Get(uint fileId)
        {
            var file = GetFile(fileId);

            CreateBucketInfo(file.URL);

            return new Attachment
            {
                FileName = file.OriginalFilename,
                ContentType = file.MimeType
            };
        }

        private static BucketInfo CreateBucketInfo(string url)
        {
            var bucket = new BucketInfo(url);

            

            return bucket;
        }

        private FileInfo GetFile(uint fileId)
        {
            var file = McmRepository.FileGet(fileId);
            var obj = McmRepository.ObjectGet(file.ObjectGuid, false, true);

            var result = obj.Files.FirstOrDefault(item => item.Id == fileId && item.Token == "S3");

            if(result == null)
                throw new ChaosDatabaseException("File cannot be downloaded through proxy");

            return result;
        }
    }

    internal class BucketInfo
    {
        public BucketInfo(string url)
        {
            var args = url.Split(';');
            Bucketname = args[0].Substring(11);
            Key = args[1].Substring(4).TrimStart('/');
        }

        public string Bucketname { get; private set; }
        public string Key { get; private set; }
    }
}
