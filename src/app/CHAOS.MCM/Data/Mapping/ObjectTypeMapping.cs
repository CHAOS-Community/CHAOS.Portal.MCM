namespace Chaos.Mcm.Data.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using CHAOS.Data;

    using Chaos.Mcm.Data.Dto;

    public class ObjectTypeMapping : IReaderMapping<ObjectType>
    {
        public IEnumerable<ObjectType> Map(IDataReader reader)
        {
            while(reader.Read())
            {
                yield return new ObjectType
                                 {
                                    ID   = reader.GetUint32("ID"),
                                    Name = reader.GetString("Name")

                                 };
            }
        }
    }
}