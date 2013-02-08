using System;
using CHAOS;
using CHAOS.Serialization;
using Chaos.Portal.Data.Dto.Standard;

namespace Chaos.Mcm.Data.Dto.Standard
{
	public class FolderInfo : Result, IFolderInfo
	{
		#region Properties

		[Serialize]
		public uint ID{ get; set; }

		[Serialize]
		public uint? ParentID{ get; set; }

		[Serialize]
		public uint FolderTypeID{ get; set; }

		[Serialize]
		public Guid SubscriptionGUID{ get; set; }

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
			ID                 = id;
			FolderTypeID       = folderTypeID;
			ParentID           = parentID;
			SubscriptionGUID   = subscriptionGUID.Value;
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