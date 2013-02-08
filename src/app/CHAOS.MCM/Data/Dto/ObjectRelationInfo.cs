namespace Chaos.Mcm.Data.Dto
{
    using System;
    using System.Xml.Linq;

    public class ObjectRelationInfo
    {
        #region Properties

        public Guid Object1Guid { get; set; }

        public Guid Object2Guid { get; set; }

        public Guid? MetadataGuid { get; set; }

        public int? Sequence { get; set; }

        public string ObjectRelationType { get; set; }

        public string LanguageCode { get; set; }

        public XDocument MetadataXml { get; set; }

        public Guid? MetadataSchemaGuid { get; set; }

        public uint ObjectRelationTypeID { get; set; }

        #endregion
        #region Business Logic

        #endregion
    }
}