namespace Chaos.Mcm.Data.Dto
{
    using System;
    using System.Xml.Linq;

    public class NewMetadata
    {
        public Guid Guid { get; set; }

        public Guid EditingUserGuid { get; set; }

        public Guid MetadataSchemaGuid { get; set; }

        public uint RevisionID { get; set; }
        
        public string LanguageCode { get; set; }

        public XDocument MetadataXml { get; set; }

        public DateTime DateCreated { get; set; }
    }
}