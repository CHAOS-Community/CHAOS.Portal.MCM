namespace Chaos.Mcm.Data.Mapping
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using CHAOS.Data;

    using Chaos.Mcm.Data.Dto;

    public class ObjectMapping : IReaderMapping<Object>
    {
        public IEnumerable<Object> Map(IDataReader reader)
        {
            var objects = new List<Object>();

            while(reader.Read())
            {
                var o = new Object
                    {
                        Guid         = reader.GetGuid("Guid"),
                        ObjectTypeID = reader.GetUint32("ObjectTypeID"),
                        DateCreated  = reader.GetDateTime("DateCreated")
                    };

                objects.Add(o);
            }
            
            reader.NextResult();

            var metadatas = reader.Map<ObjectMetadata>().ToList();

            reader.NextResult();

            var files = reader.Map<FileInfo>().ToList();

            reader.NextResult();

            var objectRelations = reader.Map<ObjectRelationInfo>().ToList();

            reader.NextResult();

            var objectFolders = reader.Map<ObjectFolder>().ToList();

            reader.NextResult();

            var accessPoints = reader.Map<ObjectAccessPoint>().ToList();

            foreach (var o in objects)
            {
                o.Files = (from f in files where f.ObjectGuid == o.Guid select f).ToList();
                o.Metadatas = ( from m in metadatas where m.ObjectGuid == o.Guid select (Metadata)m ).ToList();
                o.AccessPoints = ( from ap in accessPoints where ap.ObjectGuid == o.Guid select ap ).ToList();
                o.ObjectFolders = ( from of in objectFolders where of.ObjectGuid == o.Guid select of ).ToList();
                o.ObjectRealtionInfos = ( from or in objectRelations where or.Object1Guid == o.Guid || or.Object2Guid == o.Guid select or ).ToList();
            }
            
            return objects;
        }
    }
}
