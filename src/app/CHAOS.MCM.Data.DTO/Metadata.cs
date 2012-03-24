using System;
using System.Xml.Linq;
using Geckon;
using Geckon.Portal.Data.Result.Standard;
using Geckon.Serialization;
using Geckon.Serialization.XML;

namespace CHAOS.MCM.Data.DTO
{
	public class Metadata : Result
	{
		#region Properties

		[Serialize("GUID")]
		public UUID GUID { get; set; }

		public UUID ObjectGUID { get; set; }

		[Serialize("LanguageCode")]
		public string LanguageCode { get; set; }

		[Serialize("MetadataSchemaGUID")]
		public UUID MetadataSchemaGUID { get; set; }

		[SerializeXML(false, true)]
		[Serialize("MetadataXML")]
		public XDocument MetadataXML { get; set; }

		[Serialize("DateCreated")]
		public DateTime DateCreated { get; set; }

		#endregion
		#region Constructor

		public Metadata( Guid guid, Guid objectGUID, string languageCode, Guid metadataSchemaGUID, string metadataXML, DateTime dateCreated )
		{
			GUID               = new UUID( guid.ToByteArray() );
			ObjectGUID         = new UUID( objectGUID.ToByteArray() );
			LanguageCode       = languageCode;
			MetadataSchemaGUID = new UUID( metadataSchemaGUID.ToByteArray() );
			MetadataXML        = XDocument.Parse( metadataXML );
			DateCreated        = dateCreated;
		}

		public Metadata()
		{
			
		}

		#endregion
	}
}