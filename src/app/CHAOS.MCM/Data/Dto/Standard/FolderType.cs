using System;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Serialization;

namespace CHAOS.MCM.Data.Dto.Standard
{
	public class FolderType : Result
	{
		public FolderType( uint id, string name, DateTime dateCreated )
		{
			ID   = id;
			Name = name;
			DateCreated = dateCreated;
		}

		#region Properties

		[Serialize("ID")]
		public uint ID{ get; set; }

		[Serialize("Name")]
		public string Name{ get; set; }

		[Serialize("DateCreated")]
		public DateTime DateCreated { get; set; }

		#endregion
	}
}