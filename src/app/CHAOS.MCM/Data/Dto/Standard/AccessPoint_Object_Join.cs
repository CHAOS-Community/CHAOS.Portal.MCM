using System;

using CHAOS.Serialization;

namespace Chaos.Mcm.Data.Dto.Standard
{
    public class AccessPoint_Object_Join
    {
        #region Properties

        [Serialize("AccessPointGUID")]
        public Guid AccessPointGuid { get; set; }

        [Serialize("ObjectGUID")]
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
        #region Constructor

        public AccessPoint_Object_Join(Guid accessPointGUID, Guid objectGUID, DateTime? startDate, DateTime? endDate, DateTime dateCreated, DateTime? dateModified)
        {
            AccessPointGuid = accessPointGUID;
            ObjectGuid      = objectGUID;
            StartDate       = startDate;
            EndDate         = endDate;
            DateCreated     = dateCreated;
            DateModified    = dateModified;
        }

        public AccessPoint_Object_Join()
        {
            
        }

        #endregion
    }
}
