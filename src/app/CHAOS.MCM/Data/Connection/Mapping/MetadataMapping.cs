namespace Chaos.Mcm.Data.Connection.Mapping
{
    using System.Data;

    using Chaos.Mcm.Data.Dto;

    public class MetadataMapping : IReaderMapping
    {
        public object Map(IDataReader reader)
        {
            return new NewMetadata
                {
                    Guid               = reader.GetGuid("GUID"),
                    MetadataSchemaGuid = reader.GetGuid("MetadataSchemaGUID"),
                    RevisionID         = reader.ConvertToUint32("RevisionID"),
                    MetadataXml        = reader.ConvertToXDocument("MetadataXML"),
                    DateCreated        = reader.ConvertToDateTime("DateCreated"),
                    EditingUserGuid    = reader.GetGuid("EditingUserGUID"),
                    LanguageCode       = reader.GetString("LanguageCode"),
                };
        }
    }
}
