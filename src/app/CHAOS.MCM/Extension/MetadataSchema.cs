namespace Chaos.Mcm.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Chaos.Mcm.Data;
    using Chaos.Mcm.Permission;
    using Chaos.Portal;
    using Chaos.Portal.Data.Dto;
    using Chaos.Portal.Exceptions;
    
    public class MetadataSchema : AMcmExtension
    {
        #region Initialization

        public MetadataSchema(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
        {
        }

        public MetadataSchema()
        {
        }

        #endregion
        #region Business Logic

		public IEnumerable<Data.Dto.MetadataSchema> Get( ICallContext callContext, Guid? guid )
		{
		    var userGuid   = callContext.User.Guid;
		    var groupGuids = callContext.Groups.Select(item => item.Guid);

		    return McmRepository.MetadataSchemaGet(userGuid, groupGuids, guid, MetadataSchemaPermission.Read);
		}

        public Data.Dto.MetadataSchema Create(ICallContext callContext, string name, XDocument schemaXml, Guid? guid = null)
		{
            if( !callContext.User.SystemPermissonsEnum.HasFlag( SystemPermissons.Manage ) )
                throw new InsufficientPermissionsException("System Permissions was:" + callContext.User.SystemPermissonsEnum + ", but Manage is required" );

            McmRepository.MetadataSchemaCreate(name, schemaXml, callContext.User.Guid, guid ?? Guid.NewGuid());

            return Get(callContext, guid).First();
		}

        public Data.Dto.MetadataSchema Update(ICallContext callContext, string name, XDocument schemaXml, Guid guid)
        {
            if (HasPermissionToMetadataSchema(callContext, guid, MetadataSchemaPermission.Write)) 
                throw new InsufficientPermissionsException("User does not have permission to delete MetadataSchema");

            McmRepository.MetadataSchemaUpdate(name, schemaXml, callContext.User.Guid, guid);

            return Get(callContext, guid).First();
        }

		public ScalarResult Delete( ICallContext callContext, Guid guid )
		{
            if (HasPermissionToMetadataSchema(callContext, guid, MetadataSchemaPermission.Delete))
                throw new InsufficientPermissionsException("User does not have permission to delete MetadataSchema");

		    var result = McmRepository.MetadataSchemaDelete(guid);

            return new ScalarResult((int)result);
		}

        private bool HasPermissionToMetadataSchema(ICallContext callContext, Guid guid, MetadataSchemaPermission permission)
        {
            var userGuid   = callContext.User.Guid;
            var groupGuids = callContext.Groups.Select(item => item.Guid);

            var metadataSchemaGet = McmRepository.MetadataSchemaGet(userGuid, groupGuids, guid, permission);
            return metadataSchemaGet.Count == 0;
        }

        #endregion
    }

}
