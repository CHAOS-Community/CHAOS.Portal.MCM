namespace Chaos.Mcm.Data.Dto
{
    using System;

    using CHAOS.Serialization;

    using Chaos.Portal.Data.Dto;

    public class ObjectAccessPoint : AResult
    {
        #region Properties

        [Serialize("AccessPointGuid")]
        public Guid AccessPointGuid { get; set; }

        [Serialize("ObjectGuid")]
        public Guid ObjectGuid { get; set; }

        [Serialize("StartDate")]
        public DateTime? StartDate { get; set; }
        
        [Serialize("EndDate")]
        public DateTime? EndDate { get; set; }

        [Serialize("DateCreated")]
        public DateTime? DateCreated { get; set; }

        [Serialize("DateModified")]
        public DateTime? DateModified { get; set; }

        #endregion
    }
}
