using System;
using System.Xml.Linq;
using Geckon;
using Geckon.Portal.Data.Result.Standard;
using Geckon.Serialization;
using Geckon.Serialization.XML;

namespace CHAOS.MCM.Data.DTO
{
	public class MetadataSchema : Result
	{
		#region Properties

		[Serialize("ID")]
		public int ID { get; set; }

		// TODO: Either remove GUID or ID
		public UUID GUID { get; set; }

		[Serialize("Name")]
		public string Name { get; set; }

		[SerializeXML(false, true)]
		[Serialize("SchemaXML")]
		public XElement SchemaXML { get; set; }

		[Serialize("DateCreated")]
		public DateTime DateCreated { get; set; }

		#endregion
	}
}