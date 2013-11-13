namespace Chaos.Mcm.Extension.v5.Download
{
    public class BucketInfo
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