namespace Chaos.Mcm.Data.Dto
{
    using System;
    using System.Xml.Linq;

    using CHAOS.Serialization;

    using Chaos.Portal.Data.Dto.Standard;

    public class ObjectRelationInfo : Result
    {
        #region Properties

        [Serialize]
        public Guid Object1Guid { get; set; }

        [Serialize]
        public Guid Object2Guid { get; set; }

        [Serialize]
        public uint ObjectRelationTypeID { get; set; }

        [Serialize]
        public string ObjectRelationType { get; set; }

        [Serialize]
        public Guid? MetadataGuid { get; set; }

        [Serialize]
        public string LanguageCode { get; set; }

        [Serialize]
        public Guid? MetadataSchemaGuid { get; set; }

        [Serialize]
        public XDocument MetadataXml { get; set; }

        [Serialize]
        public int? Sequence { get; set; }

        #endregion
        #region Business Logic

        #endregion
    }
}