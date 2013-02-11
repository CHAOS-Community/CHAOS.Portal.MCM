namespace Chaos.Mcm.Data.Dto
{
    using System;

    using CHAOS.Serialization;

    using Chaos.Portal.Data.Dto.Standard;

    public class ObjectFolder : Result
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