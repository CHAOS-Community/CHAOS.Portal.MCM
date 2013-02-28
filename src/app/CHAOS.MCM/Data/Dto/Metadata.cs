namespace Chaos.Mcm.Data.Dto
{
    using System;
    using System.Xml.Linq;

    using CHAOS.Serialization;

    using Chaos.Portal.Data.Dto;

    public class Metadata : AResult
    {
        [Serialize]
        public Guid Guid { get; set; }

        [Serialize]
        public string LanguageCode { get; set; }

        [Serialize]
        public Guid MetadataSchemaGuid { get; set; }
        
        [Serialize]
        public Guid EditingUserGuid { get; set; }
        
        public uint RevisionID { get; set; }    

        [Serialize]
        public XDocument MetadataXml { get; set; }

        [Serialize]
        public DateTime DateCreated { get; set; }
    }
}