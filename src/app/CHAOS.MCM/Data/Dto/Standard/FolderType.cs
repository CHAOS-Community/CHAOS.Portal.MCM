using System;
using CHAOS.Serialization;
using Chaos.Portal.Data.Dto;

namespace Chaos.Mcm.Data.Dto.Standard
{
	public class FolderType : AResult
	{
		public FolderType( uint id, string name, DateTime dateCreated )
		{
			ID          = id;
			Name        = name;
			DateCreated = dateCreated;
		}

		#region Properties

		[Serialize]
		public uint ID{ get; set; }

		[Serialize]
		public string Name{ get; set; }

		[Serialize]
		public DateTime DateCreated { get; set; }

		#endregion
	}
}