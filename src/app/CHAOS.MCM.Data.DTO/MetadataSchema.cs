﻿using System;
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

		[Serialize("GUID")]
		public UUID GUID { get; set; }

		[Serialize("Name")]
		public string Name { get; set; }

		[SerializeXML(false, true)]
		[Serialize("SchemaXML")]
		public XDocument SchemaXML { get; set; }

		[Serialize("DateCreated")]
		public DateTime DateCreated { get; set; }

		#endregion
		#region Constructors

		public MetadataSchema( Guid guid, string name, string schemaXML, DateTime dateCreated)
		{
			GUID        = new UUID( guid.ToByteArray() );
			Name        = name;
			SchemaXML   = XDocument.Parse( schemaXML );
			DateCreated = dateCreated;
		}

		public MetadataSchema()
		{
			
		}

		#endregion
	}
}