using CHAOS.Serialization;
using Chaos.Portal.Data.Dto;

namespace Chaos.Mcm.Data.Dto.Standard
{
	public class FormatType : AResult
	{
		#region Properties

		[Serialize]
		public uint ID { get; set; }

		[Serialize]
		public string Name{ get; set; }

		#endregion
		#region Constructor

		public FormatType( uint id, string name )
		{
			ID   = id;
			Name = name;
		}

		#endregion
	}
}