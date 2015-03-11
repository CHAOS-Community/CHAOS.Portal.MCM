namespace Chaos.Mcm.Extension.v5.Download
{
    using Configuration;
    using Data.Dto;
    using Portal.Core.Data.Model;
    using Portal.Core.Exceptions;

    public class S3DownloadStrategy : IDownloadStrategy
    {
        private McmModuleConfiguration McmModuleConfiguration { get; set; }

        public S3DownloadStrategy(McmModuleConfiguration mcmModuleConfiguration)
        {
            McmModuleConfiguration = mcmModuleConfiguration;
        }

        public Attachment GetStream(FileInfo file)
        {
            var bucketInfo = CreateBucketInfo(file.URL);

            using (var client = Amazon.AWSClientFactory.CreateAmazonS3Client(McmModuleConfiguration.Aws.AccessKey, McmModuleConfiguration.Aws.SecretKey))
            {
                try
                {
                  var request = new Amazon.S3.Model.GetObjectRequest
                    {
                      BucketName = bucketInfo.Bucketname,
                      Key = bucketInfo.Key
                    };
                    var response = client.GetObject(request);

                    return new Attachment
                    {
                        FileName = file.OriginalFilename,
                        ContentType = file.MimeType,
                        Disposable = response,
                        Stream = response.ResponseStream
                    };
                }
                catch (Amazon.S3.AmazonS3Exception e)
                {
                    throw new UnhandledException(string.Format("bucket: {0}, key: {1}", bucketInfo.Bucketname, bucketInfo.Key), e);
                }
            }
        }

        public bool AllowsDownload()
        {
            return McmModuleConfiguration.HasAwsConfiguration();
        }

        private BucketInfo CreateBucketInfo(string url)
        {
            var bucket = new BucketInfo(url);

            return bucket;
        }
    }
}