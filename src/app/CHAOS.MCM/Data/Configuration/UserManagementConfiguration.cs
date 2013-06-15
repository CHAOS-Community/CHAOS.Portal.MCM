using CHAOS.Serialization;
using CHAOS.Serialization.XML;

namespace Chaos.Mcm.Data.Configuration
{
	[Serialize("UserManagementConfiguration")]
	public class UserManagementConfiguration
	{
		[SerializeXML(true)]
		public string UsersFolderName { get; set; }

		[SerializeXML(true)]
		public uint UserFolderTypeId { get; set; }

		[SerializeXML(true)]
		public uint UserObjectTypeId { get; set; }
	}
}