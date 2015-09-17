namespace Chaos.Mcm.Data.Dto.Standard
{
    using CHAOS.Serialization;

    using Chaos.Portal.Core.Data.Model;

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