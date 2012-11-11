using System;
using System.Xml.Linq;
using CHAOS.Extensions;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Serialization;
using CHAOS.Serialization.XML;

namespace CHAOS.MCM.Data.Dto.Standard
{
	public class Metadata : Result
	{
		#region Properties

		[Serialize("GUID")]
		public UUID GUID { get; set; }

		[Serialize("EditingUserGUID")]
		public UUID EditingUserGUID { get; set; }

		public UUID ObjectGUID { get; set; }

		[Serialize("LanguageCode")]
		public string LanguageCode { get; set; }

		[Serialize("MetadataSchemaGUID")]
		public UUID MetadataSchemaGUID { get; set; }

        [Serialize]
        public uint RevisionID { get; set; }

		[SerializeXML(false, true)]
		[Serialize("MetadataXML")]
		public XDocument MetadataXML { get; set; }

		[Serialize("DateCreated")]
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
            Fullname           = "CHAOS.MCM.Data.DTO.Metadata";
		}

		public Metadata() : this(Guid.Empty, Guid.Empty,null,Guid.Empty,uint.MinValue,null,DateTime.MinValue,Guid.Empty)
		{
			
		}

		#endregion
	}
}