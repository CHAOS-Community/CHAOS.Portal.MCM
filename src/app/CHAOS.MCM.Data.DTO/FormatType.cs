using Geckon.Portal.Data.Result.Standard;
using Geckon.Serialization;

namespace CHAOS.MCM.Data.DTO
{
	public class FormatType : Result
	{
		#region PropertiesMyRegion

		[Serialize("ID")]
		public int ID { get; set; }

		[Serialize("Value")]
		public string Value{ get; set; }

		#endregion
	}
}