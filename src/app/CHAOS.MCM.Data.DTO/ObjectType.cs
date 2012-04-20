using CHAOS.Portal.DTO.Standard;
using Geckon.Serialization;

namespace CHAOS.MCM.Data.DTO
{
	public  class ObjectType : Result
	{
		#region Properties

		[Serialize("ID")]
		public uint ID { get; set; }

		[Serialize("Name")]
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