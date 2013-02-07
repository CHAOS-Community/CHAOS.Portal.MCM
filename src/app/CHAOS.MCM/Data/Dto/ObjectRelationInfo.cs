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

        public override bool Equals(object obj)
        {
            var to = (ObjectRelationInfo) obj;

            return this.Object1Guid.Equals(to.Object1Guid) &&
                   this.Object2Guid.Equals(to.Object2Guid) &&
                   this.MetadataGuid.Equals(to.MetadataGuid) &&
                   this.Sequence.Equals(to.Sequence) &&
                   this.ObjectRelationType.Equals(to.ObjectRelationType) &&
                   this.ObjectRelationTypeID.Equals(to.ObjectRelationTypeID) &&
                   this.LanguageCode.Equals(to.LanguageCode) &&
                   this.MetadataSchemaGuid.Equals(to.MetadataSchemaGuid) &&
                   (this.MetadataXml.Root != null && to.MetadataXml.Root != null && this.MetadataXml.Root.Value.Equals(to.MetadataXml.Root.Value));
        }

        #endregion
    }
}