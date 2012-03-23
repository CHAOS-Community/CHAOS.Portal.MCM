﻿using System;
using System.Xml.Linq;
using Geckon.Serialization;
using Geckon.Serialization.XML;

namespace CHAOS.MCM.Data.DTO
{
	public class Format
	{
		#region Properties

		[Serialize]
		public uint ID { get; set; }
		
		[Serialize]
		public uint FormatCategoryID { get; set; }

		[Serialize]
		public string Name { get; set; }

		[SerializeXML(false, true)]
		[Serialize]
		public XDocument FormatXML { get; set; }

		[Serialize]
		public string MimeType { get; set; }

		#endregion
		#region Constructor

		public Format( uint id, uint formatCategoryID, string name, string formatXML, string mimeType )
		{
			ID               = id;
			FormatCategoryID = formatCategoryID;
			Name             = name;
			FormatXML        = string.IsNullOrEmpty( formatXML ) ? null : XDocument.Parse( formatXML );
			MimeType         = mimeType;
		}

		public Format()
		{
			
		}

		#endregion
	}
}
