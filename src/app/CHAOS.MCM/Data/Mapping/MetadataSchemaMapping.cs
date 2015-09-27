namespace Chaos.Mcm.Data.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using CHAOS.Data;

    using Dto;

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
                        Schema      = reader.GetString("SchemaXML"),
                        DateCreated = reader.GetDateTime("DateCreated"),
                    };
            }
        }
    }
}
