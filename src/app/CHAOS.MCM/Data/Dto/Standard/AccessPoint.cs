using System;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Serialization;

namespace CHAOS.MCM.Data.Dto.Standard
{
    public class AccessPoint : Result, IAccessPoint
    {
        #region Properties

        [Serialize("GUID")]
        public Guid Guid { get; set; }

        [Serialize("SubscriptionGUID")]
        public Guid SubscriptionGuid { get; set; }

        [Serialize("Name")]
        public string Name { get; set; }

        [Serialize("DateCreated")]
        public DateTime DateCreated { get; set; }

        #endregion
        #region Construction

        public AccessPoint()
        {

        }

        #endregion  
    }
}