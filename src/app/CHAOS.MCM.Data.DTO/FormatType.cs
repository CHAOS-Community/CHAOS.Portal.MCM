using CHAOS.Portal.DTO.Standard;
using Geckon.Serialization;

namespace CHAOS.MCM.Data.DTO
{
	public class FormatType : Result
	{
		#region Properties

		[Serialize("ID")]
		public uint ID { get; set; }

		[Serialize("Name")]
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