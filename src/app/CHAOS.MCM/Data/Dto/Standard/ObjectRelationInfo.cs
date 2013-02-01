namespace Chaos.Mcm.Data.Dto.Standard
{
    using System;
    using System.Xml.Linq;

    public class ObjectRelationInfo
    {
        #region Properties

        public Guid Object1Guid { get; set; }

        public Guid Object2Guid { get; set; }

        public Guid MetadataGuid { get; set; }

        public int? Sequence { get; set; }

        public string ObjectRelationType { get; set; }

        public string LanguageCode { get; set; }

        public XDocument MetadataXml { get; set; }

        public Guid MetadataSchemaGuid { get; set; }

        #endregion
        #region Business Logic

        public override bool Equals(object obj)
        {
            var to = (ObjectRelationInfo) obj;

            return Object1Guid.Equals(to.Object1Guid) &&
                   Object2Guid.Equals(to.Object2Guid) &&
                   MetadataGuid.Equals(to.MetadataGuid) &&
                   Sequence.Equals(to.Sequence) &&
                   ObjectRelationType.Equals(to.ObjectRelationType) &&
                   LanguageCode.Equals(to.LanguageCode) &&
                   MetadataSchemaGuid.Equals(to.MetadataSchemaGuid) &&
                   MetadataXml.Equals(to.MetadataXml);
        }

        #endregion
    }
}