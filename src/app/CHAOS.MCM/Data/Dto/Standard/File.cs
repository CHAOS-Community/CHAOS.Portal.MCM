using System;
using CHAOS;
using CHAOS.Serialization;
using Chaos.Portal.Data.Dto.Standard;

namespace Chaos.Mcm.Data.Dto.Standard
{
	public  class File : Result
	{
		#region Properties

		[Serialize]
		public uint ID { get; set; }

		[Serialize]
		public uint? ParentID { get; set; }

		public UUID ObjectGUID { get; set; }

		[Serialize]
		public string Filename { get; set; }

		[Serialize]
		public string OriginalFilename { get; set; }

		[Serialize]
		public uint FormatID { get; set; }

        [Serialize]
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