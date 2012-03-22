using System;
using Geckon;
using Geckon.Portal.Data.Result.Standard;
using Geckon.Serialization;

namespace CHAOS.MCM.Data.DTO
{
	public class FolderInfo : Result
	{
		#region Properties

		[Serialize("ID")]
		public int ID{ get; set; }

		[Serialize("ParentID")]
		public int? ParentID{ get; set; }

		[Serialize("FolderTypeID")]
		public int FolderTypeID{ get; set; }

		[Serialize("SubscriptionGUID")]
		public UUID SubscriptionGUID{ get; set; }

		[Serialize("Title")]
		public string Title{ get; set; }

		[Serialize("DateCreated")]
		public DateTime DateCreated{ get; set; }

		[Serialize("NumberOfSubFolders")]
		public int? NumberOfSubFolders{ get; set; }

		[Serialize("NumberOfObjects")]
		public int? NumberOfObjects{ get; set; }

		#endregion
	}
}