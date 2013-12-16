namespace Chaos.Mcm.Data.Dto.v5
{
    using System;
    using System.Xml.Linq;
    using CHAOS;
    using CHAOS.Extensions;
    using CHAOS.Serialization;
    using CHAOS.Serialization.XML;

    public class Metadata
    {
        [Serialize("GUID")]
        public UUID Guid { get; set; }

        [Serialize("EditingUserGUID")]
        public UUID EditingUserGuid { get; set; }

        public UUID ObjectGud { get; set; }

        [Serialize("LanguageCode")]
        public string LanguageCode { get; set; }

        [Serialize("MetadataSchemaGUID")]
        public UUID MetadataSchemaGuid { get; set; }

        [Serialize]
        public uint RevisionID { get; set; }

        [SerializeXML(false, true)]
        [Serialize("MetadataXML")]
        public XDocument MetadataXml { get; set; }

        [Serialize("DateCreated")]
        public DateTime DateCreated { get; set; }

        public static Metadata Create(Dto.Metadata item)
        {
            return new Metadata
            {
                Guid = item.Guid.ToUUID(),
                EditingUserGuid = item.EditingUserGuid.ToUUID(),
                LanguageCode = item.LanguageCode,
                MetadataSchemaGuid = item.MetadataSchemaGuid.ToUUID(),
                MetadataXml = item.MetadataXml,
                DateCreated = item.DateCreated
            };
        }
    }
}