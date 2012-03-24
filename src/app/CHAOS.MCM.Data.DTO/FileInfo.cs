using System;
using Geckon;
using Geckon.Portal.Data.Result.Standard;
using Geckon.Serialization;

namespace CHAOS.MCM.Data.DTO
{
	public  class FileInfo : Result
	{
		#region Properties

		[Serialize("ID")]
		public uint ID { get; set; }

		[Serialize("ParentID")]
		public uint? ParentID { get; set; }

		public UUID ObjectGUID { get; set; }

		[Serialize("Filename")]
		public string Filename { get; set; }

		[Serialize("OriginalFilename")]
		public string OriginalFilename { get; set; }

		[Serialize("Token")]
		public string Token { get; set; }

		[Serialize("URL")]
		public string URL { get; set; }

		[Serialize("FormatID")]
		public uint FormatID { get; set; }

		[Serialize("Format")]
		public string Format { get; set; }

		[Serialize("FormatCategory")]
		public string FormatCategory { get; set; }

		[Serialize("FormatType")]
		public string FormatType { get; set; }

		#endregion
		#region Constructor

		public FileInfo( uint id, uint? parentID, Guid objectGUID, string filename, string originalFilename, string token, string url, uint formatID, string format, string formatCategory, string formatType)
		{
			ID               = id;
			ParentID         = parentID;
			ObjectGUID       = new UUID( objectGUID.ToByteArray() );
			Filename         = filename;
			OriginalFilename = originalFilename;
			Token			 = token;
			URL				 = url;
			FormatID	     = formatID;
			Format           = format;
			FormatCategory   = formatCategory;
			FormatType       = formatType;
		}

		public FileInfo()
		{
			
		}

		#endregion
	}
}