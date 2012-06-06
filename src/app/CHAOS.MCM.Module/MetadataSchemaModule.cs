using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CHAOS.MCM.Data.EF;
using CHAOS.Portal.Core.Module;
using CHAOS.Portal.Core;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Portal.Exception;

namespace CHAOS.MCM.Module
{
    [Module("MCM")]
    public class MetadataSchemaModule : AMCMModule
    {
        #region Business Logic

		[Datatype("MetadataSchema", "Get")]
		public IEnumerable<Data.DTO.MetadataSchema> Get( ICallContext callContext, UUID metadataSchemaGUID )
		{
            // TODO: Limit to metadata where use has permissions
			using( var db = DefaultMCMEntities )
			{
				return db.MetadataSchema_Get( metadataSchemaGUID == null ? null : metadataSchemaGUID.ToByteArray() ).ToDTO().ToList();
			}
		}

        [Datatype("MetadataSchema", "Create")]
		public Data.DTO.MetadataSchema Create( ICallContext callContext, UUID metadataSchemaGUID, string name, string schemaXML )
		{
            // TODO: Replace with proper validation, quick fix to make sure only valid XML is inserted
            XDocument.Parse( schemaXML );

            // TODO: Limit to metadata where use has permissions
			using( var db = DefaultMCMEntities )
			{
			    var guid   = metadataSchemaGUID ?? new UUID();
				var result = db.MetadataSchema_Create( guid.ToByteArray(), name, schemaXML ).FirstOrDefault();
                
                if( result == null || !result.HasValue || result.Value != 1 )
                    throw new UnhandledException( "MetadataSchema was not created" );

                return db.MetadataSchema_Get( guid.ToByteArray() ).ToDTO().First();
			}
		}

        [Datatype("MetadataSchema", "Update")]
		public ScalarResult Update( ICallContext callContext, UUID metadataSchemaGUID, string name, string schemaXML )
		{
            // TODO: Replace with proper validation, quick fix to make sure only valid XML is inserted
            XDocument.Parse( schemaXML );

            // TODO: Limit to metadata where use has permissions
			using( var db = DefaultMCMEntities )
			{
			    var result = db.MetadataSchema_Update( metadataSchemaGUID.ToByteArray(), name, schemaXML ).FirstOrDefault();
                
                if( result == null || !result.HasValue || result.Value != 1 )
                    throw new UnhandledException( "MetadataSchema was not updated" );

                return new ScalarResult( result.Value );
			}
		}

        [Datatype("MetadataSchema", "Delete")]
		public ScalarResult Delete( ICallContext callContext, UUID metadataSchemaGUID )
		{
            // TODO: Limit to metadata where use has permissions
			using( var db = DefaultMCMEntities )
			{
			    var result = db.MetadataSchema_Delete( metadataSchemaGUID.ToByteArray() ).FirstOrDefault();
                
                if( result == null || !result.HasValue || result.Value != 1 )
                    throw new UnhandledException( "MetadataSchema was not deleted" );

                return new ScalarResult( result.Value );
			}
		}

		#endregion
    }

}
