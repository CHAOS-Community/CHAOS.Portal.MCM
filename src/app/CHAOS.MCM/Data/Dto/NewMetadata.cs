namespace Chaos.Mcm.Data.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    using Chaos.Mcm.Data.Connection;

    public class NewMetadata : IKeyValueMapper
    {
        public Guid Guid { get; set; }

        public Guid EditingUserGuid { get; set; }

        public Guid MetadataSchemaGuid { get; set; }

        public uint RevisionID { get; set; }
        
        public string LanguageCode { get; set; }

        public XDocument MetadataXml { get; set; }

        public DateTime DateCreated { get; set; }

        public void Map(KeyValuePair<string, object>[] row)
        {
            foreach(var col in row)
            {
                if (col.Key == "GUID")               Guid               = col.Value.GetGuid().Value;
                if (col.Key == "MetadataSchemaGUID") MetadataSchemaGuid = col.Value.GetGuid().Value;
                if (col.Key == "RevisionID")         RevisionID         = col.Value.GetUint() ?? 0;
                if (col.Key == "MetadataXML")        MetadataXml        = col.Value.GetXDocument();
                if (col.Key == "DateCreated")        DateCreated        = col.Value.GetDateTime().Value;
                if (col.Key == "EditingUserGUID")    EditingUserGuid    = col.Value.GetGuid().Value;
                if (col.Key == "LanguageCode")       LanguageCode       = col.Value.GetString();
            }
        }
    }
}