namespace Chaos.Mcm.Data.Dto
{
    using CHAOS;
    using CHAOS.Extensions;
    using CHAOS.Serialization;
    using CHAOS.Serialization.XML;
    using Portal.Core.Data.Model;

    [Serialize("Result")]
    public class ObjectRelation : IResult
    {
        [Serialize("Object1GUID")]
        public UUID Object1Guid { get; set; }

        [Serialize("Object2GUID")]
        public UUID Object2Guid { get; set; }

        [Serialize("ObjectRelationTypeID")]
        public uint ObjectRelationTypeID { get; set; }

        [Serialize("Sequence")]
        public int? Sequence { get; set; }

        public ObjectRelation()
        {
            Fullname = "CHAOS.MCM.Data.Dto.Standard.Object_Object_Join";
        }

        public static ObjectRelation Create(ObjectRelationInfo objectRelationInfo)
        {
            return new ObjectRelation
            {
                Object1Guid = objectRelationInfo.Object1Guid.ToUUID(),
                Object2Guid = objectRelationInfo.Object2Guid.ToUUID(),
                ObjectRelationTypeID = objectRelationInfo.ObjectRelationTypeID,
                Sequence = objectRelationInfo.Sequence
            };
        }

        [SerializeXML(true)]
        [Serialize("FullName")]
        public string Fullname { get; private set; }
    }
}