namespace Chaos.Mcm.Data.Dto
{
    using System;
    using System.Xml.Linq;

    using CHAOS.Serialization;

    using Chaos.Portal.Core.Data.Model;

    public class ObjectRelationInfo : AResult
    {
        private XDocument _metadataXml;

        #region Properties

        [Serialize]
        public Guid Object1Guid { get; set; }

        [Serialize]
        public Guid Object2Guid { get; set; }

        [Serialize]
        public uint ObjectRelationTypeID { get; set; }

        [Serialize]
        public uint Object1TypeID { get; set; }

        [Serialize]
        public uint Object2TypeID { get; set; }

        [Serialize]
        public string ObjectRelationType { get; set; }

        [Serialize]
        public Guid? MetadataGuid { get; set; }

        [Serialize]
        public string LanguageCode { get; set; }

        [Serialize]
        public Guid? MetadataSchemaGuid { get; set; }

        [Serialize]
        public XDocument MetadataXml
        {
            get
            {
                return _metadataXml;
            }
            set
            {
                _metadataXml = value;
            }
        }

        [Serialize]
        public int? Sequence { get; set; }

        #endregion
        #region Business Logic


        #endregion
    }
}