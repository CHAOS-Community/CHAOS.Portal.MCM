namespace Chaos.Mcm.Data.Connection.Mapping
{
    using System.Data;

    using Chaos.Mcm.Data.Dto;

    public class ObjectRelationInfoMapping : IReaderMapping
    {
        public object Map(IDataReader reader)
        {
            return new ObjectRelationInfo
                {
                    Object1Guid          = reader.GetGuid("Object1Guid"),
                    Object2Guid          = reader.GetGuid("Object2Guid"),
                    MetadataGuid         = reader.ConvertToGuidNullable("MetadataGuid"),
                    Sequence             = reader.ConvertToInt32Nullable("Sequence"),
                    ObjectRelationTypeID = reader.ConvertToUint32("ObjectRelationTypeID"),
                    ObjectRelationType   = reader.GetString("ObjectRelationType"),
                    LanguageCode         = reader.GetString("LanguageCode"),
                    MetadataSchemaGuid   = reader.ConvertToGuidNullable("MetadataSchemaGuid"),
                    MetadataXml          = reader.ConvertToXDocument("MetadataXml")
                };
        }
    }
}
