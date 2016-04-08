using Amazon.Runtime;
using Amazon.S3;
using Chaos.Mcm.Configuration;
using Chaos.Mcm.Data.Dto;
using Chaos.Portal.Core.Data.Model;
using Chaos.Portal.Core.Exceptions;

namespace Chaos.Mcm.Extension
{
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
			var awsCredentials = new BasicAWSCredentials(McmModuleConfiguration.Aws.AccessKey, McmModuleConfiguration.Aws.SecretKey);
			var s3Config = new AmazonS3Config
			{
				ServiceURL = bucketInfo.ServiceURL
			};

			using (var client = Amazon.AWSClientFactory.CreateAmazonS3Client(awsCredentials, s3Config))
			{
				try
				{
					var request = new Amazon.S3.Model.GetObjectRequest
					{
						BucketName = bucketInfo.Bucketname,
						Key = bucketInfo.Key,
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
				catch (System.Exception e)
				{
					throw new UnhandledException(string.Format("bucket: {0}, key: {1}, service_url: {2}", bucketInfo.Bucketname, bucketInfo.Key, bucketInfo.ServiceURL), e);
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