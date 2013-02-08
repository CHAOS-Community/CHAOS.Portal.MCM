using System;
using CHAOS;
using CHAOS.Serialization;

namespace Chaos.Mcm.Data.Dto.Standard
{
    public class AccessPoint_Object_Join
    {
        #region Properties

        [Serialize("AccessPointGUID")]
        public UUID AccessPointGUID { get; set; }

        [Serialize("ObjectGUID")]
        public UUID ObjectGUID { get; set; }

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
            AccessPointGUID = new UUID( accessPointGUID.ToByteArray() );
            ObjectGUID      = new UUID( objectGUID.ToByteArray() );
            StartDate       = startDate;
            EndDate         = endDate;
            DateCreated     = dateCreated;
            DateModified    = dateModified;
        }

        #endregion
    }
}
