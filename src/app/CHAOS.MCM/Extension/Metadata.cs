namespace Chaos.Mcm.Extension
{
    using System;
    using System.Xml.Linq;

    using Chaos.Mcm.Permission;
    using Chaos.Portal;
    using Chaos.Portal.Data.Dto.Standard;
    using Chaos.Portal.Exceptions;
    using Chaos.Portal.Extension;

    [PortalExtension(configurationName: "MCM")]
    public class Metadata : AMcmExtension
    {
        public ScalarResult Set(ICallContext callContext, Guid objectGuid, Guid metadataSchemaGuid, string languageCode, uint revisionID, XDocument metadataXml)
        {
            if(!HasPermissionToObject( callContext, objectGuid, FolderPermission.CreateUpdateObjects )) throw new InsufficientPermissionsException( "User does not have permissions to create a file for this object" );

            var metadataGuid = Guid.NewGuid();
            var userGuid     = callContext.User.Guid;

            var result  = McmRepository.MetadataSet(objectGuid, metadataGuid, metadataSchemaGuid, languageCode, revisionID, metadataXml, userGuid);
            var objects = McmRepository.ObjectGet(objectGuid, true, false, false, true, true);
            
            callContext.ViewManager.Index(objects);

            return new ScalarResult((int)result);
        }
    }
}