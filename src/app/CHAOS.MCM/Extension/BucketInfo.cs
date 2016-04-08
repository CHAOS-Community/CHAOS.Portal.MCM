namespace Chaos.Mcm.Extension
{
	public class BucketInfo
	{
		public BucketInfo(string url)
		{
			// expected format: bucketname={BASE_PATH};key={FOLDER_PATH}{FILENAME}
			var args = url.Split(';');
			Bucketname = args[0].Substring("bucketname=".Length);
			Key = args[1].Substring("key=".Length).TrimStart('/');
			ServiceURL = args[2].Substring("ServiceURL=".Length).Trim(' ');
		}

		public string Bucketname { get; private set; }
		public string Key { get; private set; }
		public string ServiceURL { get; private set; }
	}
}