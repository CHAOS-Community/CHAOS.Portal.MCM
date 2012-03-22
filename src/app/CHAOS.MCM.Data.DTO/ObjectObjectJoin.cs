using System;
using Geckon;
using Geckon.Portal.Data.Result.Standard;
using Geckon.Serialization;

namespace CHAOS.MCM.Data.DTO
{
	public class ObjectObjectJoin : Result
	{
		#region Properties

		[Serialize("Object1GUID")]
		public UUID Object1GUID { get; set; }

		[Serialize("Object2GUID")]
		public UUID Object2GUID { get; set; }

		[Serialize("ObjectRelationTypeID")]
		public int ObjectRelationTypeID { get; set; }

		[Serialize("Sequence")]
		public int? Sequence { get; set; }

		[Serialize("DateCreated")]
		public DateTime DateCreated { get; set; }

		#endregion
	}
}