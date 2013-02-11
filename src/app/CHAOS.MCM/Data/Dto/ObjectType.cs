namespace Chaos.Mcm.Data.Dto
{
    using CHAOS.Serialization;

    using Chaos.Portal.Data.Dto.Standard;

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
			this.ID   = id;
			this.Name = name;
		}

		public ObjectType()
		{
			
		}

		#endregion
	}
}