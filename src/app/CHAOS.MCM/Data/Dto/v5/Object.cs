namespace Chaos.Mcm.Data.Dto.v5
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CHAOS;
    using CHAOS.Extensions;
    using CHAOS.Serialization;
    using CHAOS.Serialization.XML;
    using Portal.Core.Data.Model;

    [Serialize("Result")]
    public class Object : IResult
    {
        [Serialize("GUID")]
        public UUID Guid { get; set; }

        [Serialize("ObjectTypeID")]
        public uint ObjectTypeId { get; set; }

        [Serialize]
        public DateTime DateCreated { get; set; }

        [Serialize]
        public IList<Metadata> Metadatas { get; set; }

        [Serialize("ObjectRelations")]
        public IList<ObjectRelation> ObjectRelations { get; set; }

        [Serialize]
        public IList<FileInfo> Files { get; set; }

        [Serialize]
        public IList<ObjectAccessPoint> AccessPoints { get; set; }

        [SerializeXML(true)]
        [Serialize("FullName")]
        public string Fullname { get; private set; }
        
        public static Object Create(Dto.Object obj)
        {
            return new Object
            {
                Guid = obj.Guid.ToUUID(),
                ObjectTypeId = obj.ObjectTypeID,
                DateCreated = obj.DateCreated,
                Metadatas = obj.Metadatas.Select(item => Metadata.Create(item)).ToList(),
                ObjectRelations = obj.ObjectRealtionInfos.Select(item => ObjectRelation.Create(item)).ToList(),
                Files = obj.Files.Select(item => FileInfo.Create(item)).ToList(),
                AccessPoints = obj.AccessPoints.Select(item => ObjectAccessPoint.Create(item)).ToList()
            };
        }

        public Object()
        {
            Fullname = "CHAOS.MCM.Data.DTO.Object";
            Metadatas = new List<Metadata>();
            ObjectRelations = new List<ObjectRelation>();
            Files = new List<FileInfo>();
            AccessPoints = new List<ObjectAccessPoint>();
        }
    }
}
