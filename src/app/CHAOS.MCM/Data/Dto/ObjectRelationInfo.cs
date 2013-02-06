namespace Chaos.Mcm.Data.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    using Chaos.Mcm.Data.Connection;

    public class ObjectRelationInfo : IKeyValueMapper
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

        public void Map(KeyValuePair<string, object>[] row)
        {
            Object1Guid          = row[0].Value.GetGuid().Value;
            Object2Guid          = row[1].Value.GetGuid().Value;
            MetadataGuid         = row[2].Value.GetGuid();
            Sequence             = row[3].Value.GetInt();
            ObjectRelationTypeID = row[4].Value.GetUint().Value;
            ObjectRelationType   = row[5].Value.GetString();
            LanguageCode         = row[6].Value.GetString();
            MetadataSchemaGuid   = row[7].Value.GetGuid();
            MetadataXml          = row[8].Value.GetXDocument();
        }

        #endregion
    }
}