namespace Chaos.Mcm.Data.Dto
{
    using System;

    using CHAOS.Serialization;

    using Chaos.Portal.Data.Dto;

    public class ObjectFolder : AResult
    {
        #region Properties

        [Serialize]
		public uint ID{ get; set; }

		[Serialize]
		public uint? ParentID{ get; set; }

		[Serialize]
		public uint FolderTypeID{ get; set; }

        [Serialize]
        public Guid ObjectGuid { get; set; }

        [Serialize]
        public uint ObjectFolderTypeID { get; set; }

		[Serialize]
		public Guid? SubscriptionGuid{ get; set; }

		[Serialize]
		public string Name{ get; set; }

		[Serialize]
		public DateTime DateCreated{ get; set; }

        #endregion
    }
}