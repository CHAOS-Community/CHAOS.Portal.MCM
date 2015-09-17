using System;

namespace Chaos.Mcm.Data.Dto
{
	public class MetadataIdentifier
	{
		public Guid ObjectGuid { get; private set; }
		public Guid MetadataSchemaGuid { get; private set; }
		public string LanguageCode { get; private set; }

		public MetadataIdentifier(Guid objectGuid, Guid metadataSchemaGuid, string languageCode)
		{
			ObjectGuid = objectGuid;
			MetadataSchemaGuid = metadataSchemaGuid;
			LanguageCode = languageCode;
		}
	}
}