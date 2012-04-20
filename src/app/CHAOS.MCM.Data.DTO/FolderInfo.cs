using System;
using CHAOS.Portal.DTO.Standard;
using Geckon;
using Geckon.Serialization;

namespace CHAOS.MCM.Data.DTO
{
	public class FolderInfo : Result
	{
		#region Properties

		[Serialize("ID")]
		public uint ID{ get; set; }

		[Serialize("ParentID")]
		public uint? ParentID{ get; set; }

		[Serialize("FolderTypeID")]
		public uint FolderTypeID{ get; set; }

		[Serialize("SubscriptionGUID")]
		public UUID SubscriptionGUID{ get; set; }

		[Serialize("Name")]
		public string Name{ get; set; }

		[Serialize("DateCreated")]
		public DateTime DateCreated{ get; set; }

		[Serialize("NumberOfSubFolders")]
		public long? NumberOfSubFolders{ get; set; }

		[Serialize("NumberOfObjects")]
		public long? NumberOfObjects { get; set; }

		#endregion
		#region constructors

		public FolderInfo( uint id, uint folderTypeID, uint? parentID, Guid? subscriptionGUID, string name, DateTime dateCreated, long? numberOfSubFolders, long? numberOfObjects )
		{
			ID                 = id;
			FolderTypeID       = folderTypeID;
			ParentID           = parentID;
			SubscriptionGUID   = subscriptionGUID.HasValue ? new UUID( subscriptionGUID.Value.ToByteArray() ) : null;
			Name               = name;
			DateCreated        = dateCreated;
			NumberOfSubFolders = numberOfSubFolders;
			NumberOfObjects    = numberOfObjects;
		}

		public FolderInfo()
		{
			
		}

		#endregion
	}
}