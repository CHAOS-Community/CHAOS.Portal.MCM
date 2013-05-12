namespace Chaos.Mcm.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Chaos.Mcm.Data;
    using Chaos.Mcm.Permission;
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Exceptions;

    public class MetadataSchema : AMcmExtension
    {
        #region Initialization

        public MetadataSchema(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
        {
        }

        public MetadataSchema(IPortalApplication portalApplication)
            : base(portalApplication)
        {
        }

        #endregion
        #region Business Logic

		public IEnumerable<Data.Dto.MetadataSchema> Get(Guid? guid )
		{
		    var userGuid   = Request.User.Guid;
		    var groupGuids = Request.Groups.Select(item => item.Guid);

		    return McmRepository.MetadataSchemaGet(userGuid, groupGuids, guid, MetadataSchemaPermission.Read);
		}

        public Data.Dto.MetadataSchema Create(string name, XDocument schemaXml, Guid? guid = null)
		{
            if (!Request.User.SystemPermissonsEnum.HasFlag(SystemPermissons.Manage)) throw new InsufficientPermissionsException("System Permissions was:" + Request.User.SystemPermissonsEnum + ", but Manage is required");

            guid = guid ?? Guid.NewGuid();

            McmRepository.MetadataSchemaCreate(name, schemaXml, Request.User.Guid, guid ?? Guid.NewGuid());

            return Get(guid).First();
		}

        public Data.Dto.MetadataSchema Update(string name, XDocument schemaXml, Guid guid)
        {
            if (HasPermissionToMetadataSchema(guid, MetadataSchemaPermission.Write)) throw new InsufficientPermissionsException("User does not have permission to delete MetadataSchema");

            McmRepository.MetadataSchemaUpdate(name, schemaXml, Request.User.Guid, guid);

            return Get(guid).First();
        }

		public ScalarResult Delete(Guid guid )
		{
            if (HasPermissionToMetadataSchema(guid, MetadataSchemaPermission.Delete)) throw new InsufficientPermissionsException("User does not have permission to delete MetadataSchema");

		    var result = McmRepository.MetadataSchemaDelete(guid);

            return new ScalarResult((int)result);
		}

        private bool HasPermissionToMetadataSchema(Guid guid, MetadataSchemaPermission permission)
        {
            var userGuid   = Request.User.Guid;
            var groupGuids = Request.Groups.Select(item => item.Guid);

            var metadataSchemaGet = McmRepository.MetadataSchemaGet(userGuid, groupGuids, guid, permission);
            return metadataSchemaGet.Count == 0;
        }

        #endregion
    }

}
