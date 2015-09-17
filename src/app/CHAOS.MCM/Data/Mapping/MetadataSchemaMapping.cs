namespace Chaos.Mcm.Data.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using CHAOS.Data;

    using Chaos.Mcm.Data.Dto;

    public class MetadataSchemaMapping : IReaderMapping<MetadataSchema>
    {
        public IEnumerable<MetadataSchema> Map(IDataReader reader)
        {
            while(reader.Read())
            {
                yield return new MetadataSchema
                    {
                        Guid        = reader.GetGuid("GUID"),
                        Name        = reader.GetString("Name"),
                        SchemaXml   = reader.GetXDocument("SchemaXML"),
                        DateCreated = reader.GetDateTime("DateCreated"),
                    };
            }
        }
    }
}
