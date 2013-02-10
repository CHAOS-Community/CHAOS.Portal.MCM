using System;
using System.Xml.Linq;
using CHAOS;
using CHAOS.Serialization;
using CHAOS.Serialization.XML;
using Chaos.Portal.Data.Dto.Standard;

namespace Chaos.Mcm.Data.Dto.Standard
{
	public class MetadataSchema : Result
	{
		#region Properties

		[Serialize("Guid")]
		public Guid Guid { get; set; }

		[Serialize("Name")]
		public string Name { get; set; }

		[SerializeXML(false, true)]
		[Serialize("SchemaXml")]
		public XDocument SchemaXml { get; set; }

		[Serialize("DateCreated")]
		public DateTime DateCreated { get; set; }

		#endregion
		#region Constructors

		public MetadataSchema( Guid guid, string name, string schemaXML, DateTime dateCreated) 
		{
			Guid        = guid;
			Name        = name;
			SchemaXml   = XDocument.Parse( schemaXML );
			DateCreated = dateCreated;
		}

		public MetadataSchema()
		{
			
		}

		#endregion
	}
}