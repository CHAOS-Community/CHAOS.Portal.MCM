using System;
using CHAOS;
using CHAOS.Serialization;
using Chaos.Portal.Data.Dto.Standard;

namespace Chaos.Mcm.Data.Dto.Standard
{
	public class Object_Object_Join : Result
	{
		#region Properties

		[Serialize]
		public UUID Object1GUID { get; set; }

		[Serialize]
		public UUID Object2GUID { get; set; }

		[Serialize]
		public uint ObjectRelationTypeID { get; set; }

		[Serialize]
		public int? Sequence { get; set; }

		[Serialize]
		public DateTime DateCreated { get; set; }

		#endregion
		#region Constructor

		public Object_Object_Join(Guid object1GUID, Guid object2GUID, uint objectRelationTypeID, int? sequence, DateTime dateCreated)
		{
			Object1GUID          = new UUID( object1GUID.ToByteArray() );
			Object2GUID          = new UUID( object2GUID.ToByteArray() );
			ObjectRelationTypeID = objectRelationTypeID;
			Sequence             = sequence;
			DateCreated          = dateCreated;
		}

		public Object_Object_Join()
		{
			
		}

		#endregion
	}
}