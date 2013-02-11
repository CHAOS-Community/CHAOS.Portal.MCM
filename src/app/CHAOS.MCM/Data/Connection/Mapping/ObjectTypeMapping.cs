namespace Chaos.Mcm.Data.Connection.Mapping
{
    using System.Collections.Generic;
    using System.Data;

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