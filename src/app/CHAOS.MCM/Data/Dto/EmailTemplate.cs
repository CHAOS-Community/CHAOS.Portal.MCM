using System.Xml.Linq;

namespace Chaos.Mcm.Data.Dto
{
	public class EmailTemplate
	{
		public string From { get; set; } 
		public string Subject { get; set; } 
		public XElement Body { get; set; } 
	}
}