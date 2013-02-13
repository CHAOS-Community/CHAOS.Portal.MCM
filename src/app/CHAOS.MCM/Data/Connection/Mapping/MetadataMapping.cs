namespace Chaos.Mcm.Data.Connection.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using CHAOS.Data;

    using Chaos.Mcm.Data.Dto;

    public class MetadataMapping : IReaderMapping<Metadata>
    {
        public IEnumerable<Metadata> Map(IDataReader reader)
        {
            while(reader.Read())
            {
                yield return new Metadata
                    {
                        Guid               = reader.GetGuid("Guid"),
                        MetadataSchemaGuid = reader.GetGuid("MetadataSchemaGUID"),
                        RevisionID         = reader.GetUint32("RevisionID"),
                        MetadataXml        = reader.GetXDocument("MetadataXML"),
                        DateCreated        = reader.GetDateTime("DateCreated"),
                        EditingUserGuid    = reader.GetGuid("EditingUserGUID"),
                        LanguageCode       = reader.GetString("LanguageCode"),
                    };
            }
        }
    }
}
