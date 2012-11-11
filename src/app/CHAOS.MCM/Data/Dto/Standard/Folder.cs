using System;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Serialization;

namespace CHAOS.MCM.Data.Dto.Standard
{
	public class Folder : Result, IFolder
	{
		#region Properties

		[Serialize("ID")]
		public uint ID { get; set; }

		[Serialize("ParentID")]
		public uint? ParentID { get; set; }

		[Serialize("FolderTypeID")]
		public uint FolderTypeID { get; set; }

		[Serialize("SubscriptionGUID")]
		public UUID SubscriptionGUID { get; set; }

		[Serialize("Name")]
		public string Name { get; set; }

		[Serialize("DateCreated")]
		public DateTime DateCreated { get; set; }

		#endregion
		#region constructors

		public Folder( uint id, uint folderTypeID, uint? parentID, Guid? subscriptionGUID, string name, DateTime dateCreated)
		{
			ID               = id;
			FolderTypeID     = folderTypeID;
			ParentID         = parentID;
			SubscriptionGUID = subscriptionGUID.HasValue ? new UUID( subscriptionGUID.Value.ToByteArray() ) : null;
			Name             = name;
			DateCreated      = dateCreated;
		}

		public Folder()
		{
			
		}

		#endregion
	}
}
