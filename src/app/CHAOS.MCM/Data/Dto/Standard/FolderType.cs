using System;
using CHAOS.Serialization;

namespace Chaos.Mcm.Data.Dto.Standard
{
    using Chaos.Portal.Core.Data.Model;

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