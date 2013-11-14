using System.Collections.Generic;
using Chaos.Mcm.Data;
using Chaos.Mcm.Data.Dto;
using Chaos.Mcm.Permission;
using Chaos.Portal.Core;

namespace Chaos.Mcm.Extension.v6
{
    public class Destination : AMcmExtension
    {
        public Destination(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
        {
        }

        public IEnumerable<DestinationInfo> Get(uint? id)
        {
            return McmRepository.DestinationGet(id);
        }
    }
}