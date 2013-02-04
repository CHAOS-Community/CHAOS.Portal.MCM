namespace Chaos.Mcm.Data.Dto.Standard
{
    using System;

    public class ObjectRelation
    {
        public Guid Object1Guid { get; set; }

        public Guid Object2Guid { get; set; }

        public Guid? MetadataGuid { get; set; }

        public uint ObjectRelationTypeID { get; set; }

        public int? Sequence { get; set; }

        public DateTime DateCreated { get; set; } 
    }
}