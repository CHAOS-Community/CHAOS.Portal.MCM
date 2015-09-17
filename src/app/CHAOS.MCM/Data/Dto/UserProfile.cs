using System;
using System.Xml.Linq;
using Chaos.Portal.Core.Data.Model;
using CHAOS.Serialization;

namespace Chaos.Mcm.Data.Dto
{
	[Serialize("UserProfile")]
	public class UserProfile : AResult
	{
		public UserProfile()
		{
			
		} 

		public UserProfile(Metadata metadata)
		{
			MetadataSchemaGuid = metadata.MetadataSchemaGuid;
			EditingUserGuid = metadata.EditingUserGuid;
			MetadataXml = metadata.MetadataXml;
			DateCreated = metadata.DateCreated;
		}

		[Serialize]
		public Guid MetadataSchemaGuid { get; set; }

		[Serialize]
		public Guid EditingUserGuid { get; set; }

		[Serialize]
		public XDocument MetadataXml { get; set; }

		[Serialize]
		public DateTime DateCreated { get; set; }
	}
}