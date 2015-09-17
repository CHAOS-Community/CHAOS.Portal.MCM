namespace Chaos.Mcm.Data.Dto
{
    using System;
    using System.Xml.Linq;

    using CHAOS.Serialization;
    using CHAOS.Serialization.XML;

    using Chaos.Portal.Core.Data.Model;

    public class MetadataSchema : AResult
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
			this.Guid        = guid;
			this.Name        = name;
			this.SchemaXml   = XDocument.Parse( schemaXML );
			this.DateCreated = dateCreated;
		}

		public MetadataSchema()
		{
			
		}

		#endregion
	}
}