using CHAOS.Portal.DTO.Standard;
using CHAOS.Serialization;

namespace CHAOS.MCM.Data.Dto.Standard
{
	public  class ObjectRelationType : Result
	{
		#region Properties

		[Serialize("ID")]
		public uint ID { get; set; }

		[Serialize("Name")]
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