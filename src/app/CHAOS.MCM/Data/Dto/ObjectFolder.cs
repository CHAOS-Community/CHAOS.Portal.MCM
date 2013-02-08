namespace Chaos.Mcm.Data.Dto
{
    using System;

    using CHAOS.Serialization;

    public class ObjectFolder
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
		public Guid SubscriptionGUID{ get; set; }

		[Serialize]
		public string Name{ get; set; }

		[Serialize]
		public DateTime DateCreated{ get; set; }

        #endregion
    }
}