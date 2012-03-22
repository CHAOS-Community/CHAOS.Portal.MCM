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

		public UUID ObjectGUID { get; set; }

		[Serialize("LanguageCode")]
		public string LanguageCode { get; set; }

		[Serialize("MetadataSchemaID")]
		public int MetadataSchemaID { get; set; }

		[SerializeXML(false, true)]
		[Serialize("MetadataXML")]
		public XDocument MetadataXML { get; set; }

		[Serialize("DateCreated")]
		public DateTime DateCreated { get; set; }

		#endregion
	}
}