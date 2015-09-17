namespace Chaos.Mcm.Data.Dto
{
    using System;

    using CHAOS.Serialization;

    using Chaos.Portal.Core.Data.Model;

    public class FolderInfo : AResult, IFolderInfo
	{
		#region Properties

		[Serialize]
		public uint ID{ get; set; }

		[Serialize]
		public uint? ParentID{ get; set; }

		[Serialize]
		public uint FolderTypeID{ get; set; }

		[Serialize]
		public Guid? SubscriptionGuid{ get; set; }

		[Serialize]
		public string Name{ get; set; }

		[Serialize]
		public DateTime DateCreated{ get; set; }

		[Serialize]
		public long? NumberOfSubFolders{ get; set; }

		[Serialize]
		public long? NumberOfObjects { get; set; }

        #endregion
		#region constructors

		public FolderInfo( uint id, uint folderTypeID, uint? parentID, Guid? subscriptionGUID, string name, DateTime dateCreated, long? numberOfSubFolders, long? numberOfObjects )
		{
			this.ID                 = id;
			this.FolderTypeID       = folderTypeID;
			this.ParentID           = parentID;
			this.SubscriptionGuid   = subscriptionGUID.Value;
			this.Name               = name;
			this.DateCreated        = dateCreated;
			this.NumberOfSubFolders = numberOfSubFolders;
			this.NumberOfObjects    = numberOfObjects;
		}

		public FolderInfo()
		{
			
		}

		#endregion
	}
}