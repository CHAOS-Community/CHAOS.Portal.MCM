namespace Chaos.Mcm.Data.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using CHAOS.Data;

    using Chaos.Mcm.Data.Dto;

    public class ObjectRelationInfoMapping : IReaderMapping<ObjectRelationInfo>
    {
        public IEnumerable<ObjectRelationInfo> Map(IDataReader reader)
        {
            while(reader.Read())
            {
                yield return new ObjectRelationInfo
                {
                    Object1Guid          = reader.GetGuid("Object1Guid"),
                    Object2Guid          = reader.GetGuid("Object2Guid"),
                    MetadataGuid         = reader.GetGuidNullable("MetadataGuid"),
                    Sequence             = reader.GetInt32Nullable("Sequence"),
                    ObjectRelationTypeID = reader.GetUint32("ObjectRelationTypeID"),
                    ObjectRelationType   = reader.GetString("ObjectRelationType"),
                    LanguageCode         = reader.GetString("LanguageCode"),
                    MetadataSchemaGuid   = reader.GetGuidNullable("MetadataSchemaGuid"),
                    MetadataXml          = reader.GetXDocument("MetadataXml")
                };
            }
        }
    }
}
