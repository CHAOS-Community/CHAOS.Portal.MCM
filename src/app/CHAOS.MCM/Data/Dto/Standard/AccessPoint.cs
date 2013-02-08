using System;
using CHAOS.Serialization;
using Chaos.Portal.Data.Dto.Standard;

namespace Chaos.Mcm.Data.Dto.Standard
{
    public class AccessPoint : Result, IAccessPoint
    {
        #region Properties

        [Serialize]
        public Guid Guid { get; set; }

        [Serialize]
        public Guid SubscriptionGuid { get; set; }

        [Serialize]
        public string Name { get; set; }

        [Serialize]
        public DateTime DateCreated { get; set; }

        #endregion
        #region Construction

        public AccessPoint()
        {

        }

        #endregion  
    }
}