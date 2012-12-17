using System;
using System.Xml.Linq;
using CHAOS;
using CHAOS.Extensions;
using CHAOS.Serialization;
using CHAOS.Serialization.XML;
using Chaos.Portal.Data.Dto.Standard;

namespace Chaos.Mcm.Data.Dto.Standard
{
	public class Metadata : Result
	{
		#region Properties

		[Serialize]
		public UUID GUID { get; set; }

		[Serialize]
		public UUID EditingUserGUID { get; set; }

		public UUID ObjectGUID { get; set; }

		[Serialize]
		public string LanguageCode { get; set; }

		[Serialize]
		public UUID MetadataSchemaGUID { get; set; }

        [Serialize]
        public uint RevisionID { get; set; }

		[SerializeXML(false, true)]
		[Serialize]
		public XDocument MetadataXML { get; set; }

		[Serialize]
		public DateTime DateCreated { get; set; }

		#endregion
		#region Constructor

		public Metadata( Guid guid, Guid objectGUID, string languageCode, Guid metadataSchemaGUID, uint revisionID, string metadataXML, DateTime dateCreated, Guid editingUserGUID )
		{
            if (!string.IsNullOrEmpty(metadataXML)) MetadataXML = XDocument.Parse(metadataXML);

			GUID               = new UUID( guid.ToByteArray() );
			ObjectGUID         = new UUID( objectGUID.ToByteArray() );
			LanguageCode       = languageCode;
			MetadataSchemaGUID = new UUID( metadataSchemaGUID.ToByteArray() );
			DateCreated        = dateCreated;
		    RevisionID         = revisionID;
			EditingUserGUID    = editingUserGUID.ToUUID();
            Fullname           = "Chaos.Mcm.Data.DTO.Metadata";
		}

		public Metadata() : this(Guid.Empty, Guid.Empty,null,Guid.Empty,uint.MinValue,null,DateTime.MinValue,Guid.Empty)
		{
			
		}

		#endregion
	}
}