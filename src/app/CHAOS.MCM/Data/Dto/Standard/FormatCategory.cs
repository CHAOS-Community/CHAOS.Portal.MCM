using CHAOS.Portal.DTO.Standard;
using CHAOS.Serialization;

namespace CHAOS.MCM.Data.Dto.Standard
{
	public class FormatCategory : Result
	{
		#region Properties

		[Serialize("ID")]
		public uint ID { get; set; }

		[Serialize("Name")]
		public string Name{ get; set; }

		#endregion
		#region Constructor

		public FormatCategory(uint id, string name)
		{
			ID   = id;
			Name = name;
		}

		#endregion
	}
}