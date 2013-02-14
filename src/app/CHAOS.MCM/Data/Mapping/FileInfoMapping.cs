namespace Chaos.Mcm.Data.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using CHAOS.Data;

    using Chaos.Mcm.Data.Dto;

    public class FileInfoMapping : IReaderMapping<FileInfo>
    {
        public IEnumerable<FileInfo> Map(IDataReader reader)
        {
            while(reader.Read())
            {
                yield return new FileInfo
                    {
                        ID                        = reader.GetUint32("FileID"),
                        ObjectGuid                = reader.GetGuid("ObjectGuid"),
                        ParentID                  = reader.GetUint32Nullable("ParentID"),
                        DestinationID             = reader.GetUint32("DestinationID"),
                        Filename                  = reader.GetString("FileName"),
                        OriginalFilename          = reader.GetString("OriginalFileName"),
                        FolderPath                = reader.GetString("FolderPath"),
                        FileDateCreated           = reader.GetDateTime("FileDateCreated"),
                        BasePath                  = reader.GetString("BasePath"),
                        StringFormat              = reader.GetString("StringFormat"),
                        AccessProviderDateCreated = reader.GetDateTime("AccessProviderDateCreated"),
                        Token                     = reader.GetString("Token"),
                        FormatID                  = reader.GetUint32("FormatID"),
                        FormatTypeName            = reader.GetString("FormatName"),
                        FormatXML                 = reader.GetString("FormatXml"),
                        MimeType                  = reader.GetString("MimeType"),
                        FormatCategoryID          = reader.GetUint32("FormatCategoryID"),
                        FormatCategory            = reader.GetString("FormatCategoryName"),
                        FormatTypeID              = reader.GetUint32("FormatTypeID"),
                        FormatType                = reader.GetString("FormatTypeName")
                    };
            }
        }
    }
}
