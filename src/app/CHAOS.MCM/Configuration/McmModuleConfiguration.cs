namespace Chaos.Mcm.Configuration
{
    using System;
    using CHAOS.Serialization;
    using CHAOS.Serialization.XML;

    [Serialize("Settings")]
    public class McmModuleConfiguration
    {
        [SerializeXML(true)]
        public String ConnectionString { get; set; }

        [SerializeXML(true)]
        public String ObjectCoreName { get; set; }

        [Serialize("AWS")]
        public AwsConfiguration Aws { get; set; }

        public bool HasAwsConfiguration()
        {
            return Aws != null;
        }
    }
}