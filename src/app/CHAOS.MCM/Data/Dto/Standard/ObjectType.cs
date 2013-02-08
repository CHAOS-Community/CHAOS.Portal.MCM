using CHAOS.Serialization;
using Chaos.Portal.Data.Dto.Standard;

namespace Chaos.Mcm.Data.Dto.Standard
{
	public  class ObjectType : Result
	{
		#region Properties

		[Serialize]
		public uint ID { get; set; }

		[Serialize]
		public string Name{ get; set; }

		#endregion
		#region Constructor

		public ObjectType( uint id, string name )
		{
			ID   = id;
			Name = name;
		}

		public ObjectType()
		{
			
		}

		#endregion
	}
}