namespace Chaos.Mcm.Data.Connection.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using Chaos.Mcm.Data.Dto;

    //todo: eliminate duplication (MetadataMapping)
    public class ObjectMetadataMapping : IReaderMapping<ObjectMetadata>
    {      
        public IEnumerable<ObjectMetadata> Map(IDataReader reader)
        {
            while(reader.Read())
            {

                yield return new ObjectMetadata
                    {
                        Guid               = reader.GetGuid("Guid"),
                        ObjectGuid         = reader.GetGuid("ObjectGuid"),
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
