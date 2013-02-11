namespace Chaos.Mcm.Data.Dto
{
    using System.Xml.Linq;

    using CHAOS.Serialization;
    using CHAOS.Serialization.XML;

    using Chaos.Portal.Data.Dto.Standard;

    public class Format : Result
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
			this.ID               = id;
			this.FormatCategoryID = formatCategoryID;
			this.Name             = name;
			this.FormatXml        = string.IsNullOrEmpty( formatXML ) ? null : XDocument.Parse( formatXML );
			this.MimeType         = mimeType;
            this.Extension        = extenison;
		}

		public Format()
		{
			
		}

		#endregion
	}
}
