namespace Chaos.Mcm.Data.Connection.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using Chaos.Mcm.Data.Dto;

    public class MetadataMapping : IReaderMapping<NewMetadata>
    {
        public IEnumerable<NewMetadata> Map(IDataReader reader)
        {
            while(reader.Read())
            {
                yield return new NewMetadata
                    {
                        Guid               = reader.GetGuid("GUID"),
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
