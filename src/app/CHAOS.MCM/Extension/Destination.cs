namespace Chaos.Mcm.Extension
{
    using System.Collections.Generic;

    using Chaos.Mcm.Data;
    using Chaos.Mcm.Data.Dto;
    using Chaos.Mcm.Permission;
    using Chaos.Portal.Core;

    public class Destination : AMcmExtension
    {
        public Destination(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
        {
        }

        public Destination(IPortalApplication portalApplication) : base(portalApplication)
        {
        }

        public IEnumerable<DestinationInfo> Get(uint? id)
        {
            return McmRepository.DestinationGet(id);
        }
    }
}