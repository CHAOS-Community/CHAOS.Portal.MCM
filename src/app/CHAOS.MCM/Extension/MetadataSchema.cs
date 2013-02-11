namespace Chaos.Mcm.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Chaos.Mcm.Permission;
    using Chaos.Portal;
    using Chaos.Portal.Data.Dto.Standard;
    using Chaos.Portal.Exceptions;
    
    public class MetadataSchema : AMcmExtension
    {
        #region Business Logic

		public IEnumerable<Data.Dto.Standard.MetadataSchema> Get( ICallContext callContext, Guid? guid )
		{
		    var userGuid   = callContext.User.Guid;
		    var groupGuids = callContext.Groups.Select(item => item.Guid);

		    return McmRepository.MetadataSchemaGet(userGuid, groupGuids, guid, MetadataSchemaPermission.Read);
		}

        public Data.Dto.Standard.MetadataSchema Set(ICallContext callContext, string name, XDocument schemaXml, Guid guid = new Guid())
		{
            if( !callContext.User.SystemPermissonsEnum.HasFlag( SystemPermissons.Manage ) )
                throw new InsufficientPermissionsException( "Manage permissions are required to create metadata schemas" );

            McmRepository.MetadataSchemaSet(name, schemaXml, callContext.User.Guid, guid);

            return Get(callContext, guid).First();
		}

		public ScalarResult Delete( ICallContext callContext, Guid guid )
		{
            var userGuid   = callContext.User.Guid;
            var groupGuids = callContext.Groups.Select(item => item.Guid);

            var results = McmRepository.MetadataSchemaGet(userGuid, groupGuids, guid, MetadataSchemaPermission.Delete);

            if (!results.Any())
                throw new InsufficientPermissionsException("User does not have permission to delete MetadataSchema");

		    var result = McmRepository.MetadataSchemaDelete(guid);

            return new ScalarResult((int)result);
		}

		#endregion
    }

}
