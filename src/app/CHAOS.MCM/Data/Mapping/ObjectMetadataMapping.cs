using System.Xml.Linq;

namespace Chaos.Mcm.Data.Mapping
{
	using System.Collections.Generic;
	using System.Data;
	using CHAOS.Data;
	using Chaos.Mcm.Data.Dto;

	//todo: eliminate duplication (MetadataMapping)
	public class ObjectMetadataMapping : IReaderMapping<ObjectMetadata>
	{
		public IEnumerable<ObjectMetadata> Map(IDataReader reader)
		{
			while (reader.Read())
			{
				XDocument metadataXml;

				try
				{
					metadataXml = reader.GetXDocument("MetadataXML");
				}
				catch (System.Xml.XmlException)
				{
					continue;
				}

				yield return new ObjectMetadata
				{
					Guid = reader.GetGuid("Guid"),
					ObjectGuid = reader.GetGuid("ObjectGuid"),
					MetadataSchemaGuid = reader.GetGuid("MetadataSchemaGUID"),
					RevisionID = reader.GetUint32("RevisionID"),
					MetadataXml = metadataXml,
					DateCreated = reader.GetDateTime("DateCreated"),
					EditingUserGuid = reader.GetGuid("EditingUserGUID"),
					LanguageCode = reader.GetString("LanguageCode"),
				};
			}
		}
	}
}