namespace Chaos.Mcm.Data.Dto.Standard
{
    using System;

    using CHAOS.Serialization;
    using Chaos.Portal.Data.Dto.Standard;

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
		public Guid? SubscriptionGuid { get; set; }

		[Serialize]
		public string Name { get; set; }

		[Serialize]
		public DateTime DateCreated { get; set; }

		#endregion
		#region constructors

		public Folder( uint id, uint folderTypeID, uint? parentID, Guid? subscriptionGuid, string name, DateTime dateCreated)
		{
			ID               = id;
			FolderTypeID     = folderTypeID;
			ParentID         = parentID;
			SubscriptionGuid = subscriptionGuid;
			Name             = name;
			DateCreated      = dateCreated;
		}

		public Folder()
		{
			
		}

		#endregion
	}
}
