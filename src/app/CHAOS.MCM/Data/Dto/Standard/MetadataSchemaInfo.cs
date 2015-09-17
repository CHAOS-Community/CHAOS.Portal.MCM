using System;
using System.Collections.Generic;
using CHAOS.Serialization;

namespace Chaos.Mcm.Data.Dto.Standard
{
	public class MetadataSchemaInfo : MetadataSchema
	{
		#region Properties

        [Serialize]
        public IEnumerable<EntityPermission> UserPermissions { get; set; }

        [Serialize]
        public IEnumerable<EntityPermission> GroupPermissions { get; set; }

		#endregion
		#region Constructors

		public MetadataSchemaInfo( Guid guid, string name, string schemaXML, DateTime dateCreated) : base(guid, name, schemaXML, dateCreated)
		{

		}

        public MetadataSchemaInfo()
		{
			
		}

		#endregion
	}
}