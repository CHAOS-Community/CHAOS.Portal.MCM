using CHAOS.Serialization;

namespace Chaos.Mcm.Data.Dto.Standard
{
    using Chaos.Portal.Core.Data.Model;

    public class FormatCategory : AResult
	{
		#region Properties

		[Serialize]
		public uint ID { get; set; }

		[Serialize]
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