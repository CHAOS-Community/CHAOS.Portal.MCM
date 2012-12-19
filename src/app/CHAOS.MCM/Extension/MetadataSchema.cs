using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CHAOS;
using CHAOS.Extensions;
using Chaos.Mcm.Data.EF;
using Chaos.Mcm.Permission;
using CHAOS.Portal.Exception;
using Chaos.Portal;
using Chaos.Portal.Data.Dto.Standard;
using MetadataSchema = Chaos.Mcm.Data.Dto.Standard.MetadataSchema;

namespace Chaos.Mcm.Extension
{
    public class MetadataSchema : AMcmExtension
    {
        #region Business Logic

		public IEnumerable<Chaos.Mcm.Data.Dto.Standard.MetadataSchema> Get( ICallContext callContext, UUID metadataSchemaGUID )
		{
		    var userGuid           = callContext.User.GUID.ToGuid();
		    var groupGuids         = callContext.Groups.Select(item => item.GUID.ToGuid());
		    var metadataSchemaGuid = metadataSchemaGUID == null ? (Guid?) null : metadataSchemaGUID.ToGuid();

		    return McmRepository.GetMetadataSchema(userGuid, groupGuids, metadataSchemaGuid, MetadataSchemaPermission.Read);
		}

		public Chaos.Mcm.Data.Dto.Standard.MetadataSchema Create( ICallContext callContext, UUID metadataSchemaGUID, string name, string schemaXML )
		{
            if( !callContext.User.SystemPermissonsEnum.HasFlag( SystemPermissons.Manage ) )
                throw new InsufficientPermissionsException( "Manage permissions are required to create metadata schemas" );
            
            // TODO: Replace with proper validation, quick fix to make sure only valid XML is inserted
            XDocument.Parse( schemaXML );

			using( var db = DefaultMCMEntities )
			{
			    var guid   = metadataSchemaGUID ?? new UUID();
				var result = db.MetadataSchema_Create( guid.ToByteArray(), name, schemaXML, callContext.User.GUID.ToByteArray() ).FirstOrDefault();
                
                if( result == null || !result.HasValue || result.Value != 1 )
                    throw new UnhandledException( "MetadataSchema was not created" );

                return db.MetadataSchema_Get( null, null, guid.ToByteArray(), 0x1 ).ToDto().First();
			}
		}

		public ScalarResult Update( ICallContext callContext, UUID metadataSchemaGUID, string name, string schemaXML )
		{
            // TODO: Replace with proper validation, quick fix to make sure only valid XML is inserted
            XDocument.Parse( schemaXML );

			using( var db = DefaultMCMEntities )
			{
                var guids = string.Join( ",",callContext.Groups.Select( guid => guid.GUID.ToString().Replace("-","") ) );
			    var getWithPermission = db.MetadataSchema_Get( callContext.User.GUID.ToByteArray(), guids, metadataSchemaGUID.ToByteArray(), 0x4 ).FirstOrDefault();

                if( getWithPermission == null )
                    throw new InsufficientPermissionsException( "User does not have permission to delete MetadataSchema" );

			    var result = db.MetadataSchema_Update( metadataSchemaGUID.ToByteArray(), name, schemaXML ).FirstOrDefault();
                
                if( result == null || !result.HasValue || result.Value != 1 )
                    throw new UnhandledException( "MetadataSchema was not updated" );

                return new ScalarResult( result.Value );
			}
		}

		public ScalarResult Delete( ICallContext callContext, UUID metadataSchemaGUID )
		{
			using( var db = DefaultMCMEntities )
			{
                var guids = string.Join( ",",callContext.Groups.Select( guid => guid.GUID.ToString().Replace("-","") ) );
			    var getWithPermission = db.MetadataSchema_Get( callContext.User.GUID.ToByteArray(), guids, metadataSchemaGUID.ToByteArray(), 0x2 ).FirstOrDefault();

                if( getWithPermission == null )
                    throw new InsufficientPermissionsException( "User does not have permission to delete MetadataSchema" );

                var result = db.MetadataSchema_Delete( metadataSchemaGUID.ToByteArray() ).FirstOrDefault();

                if (result == null || !result.HasValue || result.Value != 1)
                    throw new UnhandledException( "MetadataSchema was not deleted" );

                return new ScalarResult( result.Value );
			}
		}

		#endregion
    }

}
