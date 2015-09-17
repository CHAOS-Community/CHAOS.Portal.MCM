namespace Chaos.Mcm.Data.Dto.v5
{
    using System;
    using CHAOS;
    using CHAOS.Extensions;
    using CHAOS.Serialization;
    using CHAOS.Serialization.XML;
    using Portal.Core.Data.Model;

    [Serialize("AccessPoint_Object_Join")]
    public class ObjectAccessPoint : IResult
    {
        [SerializeXML(true)]
        [Serialize("FullName")]
        public string Fullname { get; private set; }

        [Serialize("AccessPointGUID")]
        public UUID AccessPointGuid { get; set; }

        [Serialize("ObjectGUID")]
        public UUID ObjectGuid { get; set; }

        [Serialize("StartDate")]
        public DateTime? StartDate { get; set; }

        [Serialize("EndDate")]
        public DateTime? EndDate { get; set; }

        [Serialize("DateCreated")]
        public DateTime? DateCreated { get; set; }

        [Serialize("DateModified")]
        public DateTime? DateModified { get; set; }

        public ObjectAccessPoint()
        {
            Fullname = "Chaos.Mcm.Data.Dto.v5.ObjectAccessPoint";
        }

        public static ObjectAccessPoint Create(Dto.ObjectAccessPoint item)
        {
            return new ObjectAccessPoint
                {
                    AccessPointGuid = item.AccessPointGuid.ToUUID(),
                    ObjectGuid = item.ObjectGuid.ToUUID(),
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    DateCreated = item.DateCreated,
                    DateModified = item.DateModified
                };
        }
    }
}