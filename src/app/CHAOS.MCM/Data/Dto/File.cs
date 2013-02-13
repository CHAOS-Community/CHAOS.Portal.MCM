namespace Chaos.Mcm.Data.Dto
{
    using System;

    using CHAOS.Serialization;

    using Chaos.Portal.Data.Dto.Standard;

    public class File : Result
	{
		#region Properties

		[Serialize]
		public uint ID { get; set; }

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
			this.ID               = id;
			this.ParentID         = parentID;
			this.ObjectGuid       = objectGuid;
			this.Filename         = filename;
			this.OriginalFilename = originalFilename;
			this.FormatID	     = formatID;
            this.FolderPath       = folderPath;
		}

		public File()
		{
			
		}

		#endregion
	}
}