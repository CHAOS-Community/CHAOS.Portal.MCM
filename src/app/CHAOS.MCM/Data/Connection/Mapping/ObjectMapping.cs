namespace Chaos.Mcm.Data.Connection.Mapping
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Chaos.Mcm.Data.Dto;
    using Chaos.Mcm.Data.Connection;

    public class ObjectMapping : IReaderMapping<NewObject>
    {
        public IEnumerable<NewObject> Map(IDataReader reader)
        {
            var objects = new List<NewObject>();

            while(reader.Read())
            {
                var o = new NewObject
                    {
                        Guid         = reader.GetGuid("GUID"),
                        ObjectTypeID = reader.GetUint32("ObjectTypeID"),
                        DateCreated  = reader.GetDateTime("DateCreated")
                    };

                objects.Add(o);
            }
            
            reader.NextResult();
            
            var metadatas = new ObjectMetadataMapping().Map(reader);
            
            foreach (var o in objects)
            {
                o.Metadatas = (from m in metadatas where m.ObjectGuid == o.Guid select (NewMetadata) m).ToList();
            }
            
            reader.NextResult();

            var files = new FileInfoMapping().Map(reader);

            foreach (var o in objects)
            {
                o.Files = (from f in files where f.ObjectGUID == o.Guid select f).ToList();
            }

            reader.NextResult();

            var objectRelations = new ObjectRelationInfoMapping().Map( reader );

            foreach(var o in objects)
            {
                o.ObjectRelationInfos = ( from or in objectRelations where or.Object1Guid == o.Guid || or.Object2Guid == o.Guid select or ).ToList();
            }
            
            reader.NextResult();

            var objectFolders = new ObjectFolderMapping().Map( reader );

            foreach(var o in objects)
            {
                o.ObjectFolders = ( from of in objectFolders where of.ObjectGuid == o.Guid select of ).ToList();
            }

            reader.NextResult();

            var accessPoints = new AccesspointObjectJoinMapping().Map( reader );

            foreach(var o in objects)
            {
                o.AccessPoints = ( from ap in accessPoints where ap.ObjectGuid == o.Guid select ap ).ToList();
            }
            
            return objects;
        }
    }
}
