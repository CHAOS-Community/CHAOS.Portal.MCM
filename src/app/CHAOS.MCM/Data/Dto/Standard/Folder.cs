using System;
using CHAOS;
using CHAOS.Serialization;
using Chaos.Portal.Data.Dto.Standard;

namespace Chaos.Mcm.Data.Dto.Standard
{
	public class Folder : Result, IFolder
	{
		#region Properties

		[Serialize]
		public uint ID { get; set; }

		[Serialize]
		public uint? ParentID { get; set; }

		[Serialize]
		public uint FolderTypeID { get; set; }

		[Serialize]
		public UUID SubscriptionGUID { get; set; }

		[Serialize]
		public string Name { get; set; }

		[Serialize]
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
