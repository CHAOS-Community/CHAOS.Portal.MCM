using System.Xml.Linq;
using CHAOS.Serialization;
using CHAOS.Serialization.XML;

namespace Chaos.Mcm.Data.Dto.Standard
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
		public XDocument FormatXml { get; set; }

		[Serialize]
		public string MimeType { get; set; }

        [Serialize]
        public string Extension { get; set; }

		#endregion
		#region Constructor

		public Format( uint id, uint formatCategoryID, string name, string formatXML, string mimeType, string extenison )
		{
			ID               = id;
			FormatCategoryID = formatCategoryID;
			Name             = name;
			this.FormatXml        = string.IsNullOrEmpty( formatXML ) ? null : XDocument.Parse( formatXML );
			MimeType         = mimeType;
            Extension        = extenison;
		}

		public Format()
		{
			
		}

		#endregion
	}
}
