using CHAOS.Serialization;
using Chaos.Portal.Data.Dto.Standard;

namespace Chaos.Mcm.Data.Dto.Standard
{
	public  class ObjectRelationType : Result
	{
		#region Properties

		[Serialize]
		public uint ID { get; set; }

		[Serialize]
		public string Name { get; set; }

		#endregion
		#region Constructors

		public ObjectRelationType( uint id, string name )
		{
			ID   = id;
			Name = name;
		}

		public ObjectRelationType()
		{
			
		}

		#endregion
	}
}