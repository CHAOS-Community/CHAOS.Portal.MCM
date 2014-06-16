using Chaos.Mcm.Data.Configuration;

namespace Chaos.Mcm.Configuration
{
    using System;
    using CHAOS.Serialization;
    using CHAOS.Serialization.XML;
    using Portal.Core.Module;

    [Serialize("Settings")]
    public class McmModuleConfiguration : IModuleSettings
    {
        [SerializeXML(true)]
        public String ConnectionString { get; set; }

        [SerializeXML(true)]
        public String ObjectCoreName { get; set; }

        [Serialize("AWS")]
        public AwsConfiguration Aws { get; set; }

		[Serialize]
		public UserManagementConfiguration UserManagement { get; set; }

        public bool HasAwsConfiguration()
        {
            return Aws != null;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(ConnectionString);
        }
    }
}