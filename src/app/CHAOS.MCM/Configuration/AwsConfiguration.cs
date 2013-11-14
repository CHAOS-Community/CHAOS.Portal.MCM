namespace Chaos.Mcm.Configuration
{
    using System;
    using CHAOS.Serialization.XML;

    public class AwsConfiguration
    {
        [SerializeXML(true)]
        public String AccessKey { get; set; }

        [SerializeXML(true)]
        public String SecretKey { get; set; }
    }
}