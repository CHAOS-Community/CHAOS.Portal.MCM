using Newtonsoft.Json;

namespace Chaos.Mcm.Data.Dto
{
    using System;

    using CHAOS.Serialization;

    using Chaos.Portal.Core.Data.Model;

    public class File : AResult
	{
		#region Properties

		[Serialize, JsonProperty("Id")]
		public uint Id { get; set; }

		[Serialize]
		public uint? ParentID { get; set; }

		public Guid ObjectGuid { get; set; }

        [Serialize]
        public uint DestinationID { get; set; }

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

		public File( uint id, uint? parentID, Guid objectGuid, string filename, string originalFilename, uint formatID, string folderPath )
		{
			Id               = id;
			ParentID         = parentID;
			ObjectGuid       = objectGuid;
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