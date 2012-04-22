using System;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Serialization;

namespace CHAOS.MCM.Data.DTO
{
	public  class File : Result
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

		[Serialize("FormatID")]
		public uint FormatID { get; set; }

        [Serialize("OriginalFilename")]
		public string FolderPath { get; set; }

		#endregion
		#region Constructor

		public File( uint id, uint? parentID, Guid objectGUID, string filename, string originalFilename, uint formatID, string folderPath )
		{
			ID               = id;
			ParentID         = parentID;
			ObjectGUID       = new UUID( objectGUID.ToByteArray() );
			Filename         = filename;
			OriginalFilename = originalFilename;
			FormatID	     = formatID;
            FolderPath       = folderPath;
		}

		public File()
		{
			
		}

		#endregion
	}
}