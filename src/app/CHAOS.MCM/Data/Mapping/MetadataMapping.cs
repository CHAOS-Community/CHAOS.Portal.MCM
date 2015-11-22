using System.Xml.Linq;

namespace Chaos.Mcm.Data.Mapping
{
  using System.Collections.Generic;
  using System.Data;
  using CHAOS.Data;
  using Chaos.Mcm.Data.Dto;

  public class MetadataMapping : IReaderMapping<Metadata>
  {
    public IEnumerable<Metadata> Map(IDataReader reader)
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

	      yield return new Metadata
          {
            Guid = reader.GetGuid("Guid"),
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