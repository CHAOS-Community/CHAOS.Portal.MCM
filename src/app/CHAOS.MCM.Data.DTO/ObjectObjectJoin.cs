using System;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Serialization;

namespace CHAOS.MCM.Data.DTO
{
	public class Object_Object_Join : Result
	{
		#region Properties

		[Serialize("Object1GUID")]
		public UUID Object1GUID { get; set; }

		[Serialize("Object2GUID")]
		public UUID Object2GUID { get; set; }

		[Serialize("ObjectRelationTypeID")]
		public uint ObjectRelationTypeID { get; set; }

		[Serialize("Sequence")]
		public int? Sequence { get; set; }

		[Serialize("DateCreated")]
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