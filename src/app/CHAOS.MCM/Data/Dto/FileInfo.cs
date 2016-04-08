using Newtonsoft.Json;

namespace Chaos.Mcm.Data.Dto
{
    using System;

    using CHAOS;
    using CHAOS.Serialization;

    using Chaos.Portal.Core.Data.Model;

    public  class FileInfo : AResult
	{
		#region Properties

		[Serialize("ID")]
      [JsonProperty("Id")]
		public uint Identifier { get; set; }

		[Serialize]
		public uint? ParentID { get; set; }

		public Guid ObjectGuid { get; set; }

		[Serialize]
		public string Filename { get; set; }

		[Serialize]
		public string OriginalFilename { get; set; }

		[Serialize]
		public string Token { get; set; }

	    [Serialize]
	    public string URL
	    {
	        get
	        {
	            var url = new System.Text.StringBuilder( this.StringFormat );
                
                url.Replace( "{BASE_PATH}", this.BasePath ?? "{BASE_PATH_MISSING}" );
                url.Replace( "{FOLDER_PATH}", string.IsNullOrEmpty( System.IO.Path.GetPathRoot( this.BasePath ) ) ? this.FolderPath.Replace( "\\", "/" ) : this.FolderPath.Replace( "/", "\\" ) );
                url.Replace( "{FILENAME}", this.Filename ?? "{FILENAME_MISSING}");
                url.Replace( "{FILE_ID}", this.Identifier.ToString() ?? "{FILE_ID_MISSING}");
                url.Replace( "{SESSION_GUID}", this.SessionGUID == null ? "{SESSION_GUID_MISSING}" : this.SessionGUID.ToString() );
                url.Replace( "{OBJECT_GUID}", this.ObjectGuid == null ? "{OBJECT_GUID_MISSING}" : this.ObjectGuid.ToString() );

	            return url.ToString();
	        }
	    }

		[Serialize]
		public uint FormatID { get; set; }

		[Serialize]
		public string Format { get; set; }

		[Serialize]
		public string FormatCategory { get; set; }

		[Serialize]
		public string FormatType { get; set; }

        public uint DestinationID { get; set; }
        public string FolderPath { get; set; }
        public DateTime FileDateCreated { get; set; }
        public string BasePath { get; set; }
        public DateTime AccessProviderDateCreated { get; set; }
        public string FormatXML { get; set; }
        public string MimeType { get; set; }
        public uint FormatCategoryID { get; set; }
        public string StringFormat { get; set; }
        public uint FormatTypeID { get; set; }
        public string FormatTypeName { get; set; }

        public UUID SessionGUID { get; set; }

		#endregion
		#region Constructor

        public FileInfo( uint fileId, Guid objectGUID, uint? parentId, uint destinationId, string fileName, string originalFileName, string folderPath, DateTime fileDateCreated, string basePath, string stringFormat, DateTime accessProviderDateCreated, string token, uint formatId, string formatName, string formatXML, string mimeType, uint formatCategoryId, string formatCategoryName, uint formatTypeId, string formatTypeName )
        {
            Identifier                     = fileId;
            ParentID               = parentId;
            ObjectGuid             = objectGUID;
            Filename               = fileName;
            OriginalFilename       = originalFileName;
            Token                  = token;
            FormatID               = formatId;
            Format                 = formatName;
            StringFormat           = stringFormat;
            FormatType             = formatTypeName;
            DestinationID          = destinationId;
            FolderPath             = folderPath;
            FileDateCreated        = fileDateCreated;
            BasePath               = basePath;
            AccessProviderDateCreated = accessProviderDateCreated;
            FormatXML              = formatXML;
            MimeType               = mimeType;
            FormatCategoryID       = formatCategoryId;
            FormatCategory         = formatCategoryName;
            FormatTypeID           = formatTypeId;
            FormatTypeName         = formatTypeName;
        }

        public FileInfo( uint fileId, Guid objectGUID, uint? parentId, uint destinationId, string fileName, string originalFileName, string folderPath, DateTime fileDateCreated, string basePath, string stringFormat, DateTime accessProviderDateCreated, string token, uint formatId, string formatName, string formatXML, string mimeType, uint formatCategoryId, string formatCategoryName, uint formatTypeId, string formatTypeName, UUID sessionGUID ) : this ( fileId,objectGUID, parentId,destinationId,fileName,originalFileName,folderPath,fileDateCreated,basePath,stringFormat, accessProviderDateCreated, token,formatId,formatName,formatXML,mimeType,formatCategoryId,formatCategoryName,formatTypeId,formatTypeName )
        {
            this.SessionGUID = sessionGUID;
        }

		public FileInfo() : this(uint.MinValue,Guid.Empty,null,uint.MinValue,null,null,null,DateTime.MinValue,null,null,DateTime.MinValue,null,uint.MinValue,null,null,null,uint.MinValue,null,uint.MinValue,null)
		{
			
		}

	    #endregion
	}
}