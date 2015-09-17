using CHAOS.Serialization;

namespace Chaos.Mcm.Data.Dto.Standard
{
    using Chaos.Portal.Core.Data.Model;

    public  class ObjectRelationType : AResult
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