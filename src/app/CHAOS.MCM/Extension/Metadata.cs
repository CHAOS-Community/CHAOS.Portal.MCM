namespace Chaos.Mcm.Extension
{
    using System;
    using System.Xml.Linq;

    using Chaos.Mcm.Data;
    using Chaos.Mcm.Permission;
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Exceptions;

    public class Metadata : AMcmExtension
    {
        #region Initialization

        public Metadata(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
        {
        }

        #endregion

        public ScalarResult Set(Guid objectGuid, Guid metadataSchemaGuid, string languageCode, uint revisionID, XDocument metadataXml)
        {
            if(!HasPermissionToObject(objectGuid, FolderPermission.CreateUpdateObjects )) throw new InsufficientPermissionsException( "User does not have permissions to set metadata on object or the object doesn't exist" );

            var metadataGuid = Guid.NewGuid();
            var userGuid     = Request.User.Guid;

            var result  = McmRepository.MetadataSet(objectGuid, metadataGuid, metadataSchemaGuid, languageCode, revisionID, metadataXml, userGuid);
            var objects = McmRepository.ObjectGet(objectGuid, true, false, false, true, true);
            
            ViewManager.Index(objects);

            return new ScalarResult((int)result);
        }
    }
}