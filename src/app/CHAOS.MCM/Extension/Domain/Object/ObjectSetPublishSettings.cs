namespace Chaos.Mcm.Extension.Domain.Object
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Permission;
    using Portal.Core.Exceptions;
    using Portal.Core.Indexing.View;

    public class ObjectSetPublishSettings : ExtensionHelperBase, IObjectSetPublishSettings
    {
        public ObjectSetPublishSettings(IMcmRepository mcmRepository, IPermissionManager permissionManager, IViewManager viewManager)
            : base(mcmRepository, permissionManager, viewManager)
        {
        }

        public uint SetPublishSettings(Guid objectGuid, Guid accessPointGuid, DateTime? startDate, DateTime? endDate, Guid userGuid, IEnumerable<Guid> groupGuids)
        {
            if (!McmRepository.AccessPointGet(accessPointGuid, userGuid, groupGuids, (uint)AccessPointPermission.Write).Any())
                throw new InsufficientPermissionsException("User does not have permission to set publish settings for object in accessPoint");

            var result = McmRepository.AccessPointPublishSettingsSet(accessPointGuid, objectGuid, startDate, endDate);

            if (result == 1)
            {
                var obj = McmRepository.ObjectGet(objectGuid, true, true, true, true, true);

                ViewManager.Index(obj);
            }

            return result;
        }
    }

    public interface IObjectSetPublishSettings
    {
        uint SetPublishSettings(Guid objectGuid, Guid accessPointGuid, DateTime? startDate, DateTime? endDate, Guid userGuid, IEnumerable<Guid> groupGuids);
    }
}
