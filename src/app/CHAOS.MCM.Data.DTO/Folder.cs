using System;
using Geckon;
using Geckon.Serialization;
using CHAOS.Portal.DTO.Standard;

namespace CHAOS.MCM.Data.DTO
{
	public class Folder : Result
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
			ID = id;
			FolderTypeID = folderTypeID;
			ParentID = parentID;
			SubscriptionGUID = subscriptionGUID.HasValue ? new UUID( subscriptionGUID.Value.ToByteArray() ) : null;
			Name = name;
			DateCreated = dateCreated;
		}

		public Folder()
		{
			
		}

		#endregion
	}
}
