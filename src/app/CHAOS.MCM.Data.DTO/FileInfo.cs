using Geckon.Portal.Data.Result.Standard;
using Geckon.Serialization;

namespace CHAOS.MCM.Data.DTO
{
	public  class FileInfo : Result
	{
		#region Properties

		[Serialize("ID")]
		public int ID { get; set; }

		[Serialize("ParentID")]
		public int? ParentID { get; set; }

		public int ObjectID { get; set; }

		[Serialize("Filename")]
		public string Filename { get; set; }

		[Serialize("OriginalFilename")]
		public string OriginalFilename { get; set; }

		[Serialize("Token")]
		public string Token { get; set; }

		[Serialize("URL")]
		public string URL { get; set; }

		[Serialize("FormatID")]
		public int FormatID { get; set; }

		[Serialize("Format")]
		public string Format { get; set; }

		[Serialize("FormatCategory")]
		public string FormatCategory { get; set; }

		[Serialize("FormatType")]
		public string FormatType { get; set; }

		#endregion
	}
}