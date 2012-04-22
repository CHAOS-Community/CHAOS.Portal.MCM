using CHAOS.Portal.DTO.Standard;
using CHAOS.Serialization;

namespace CHAOS.MCM.Data.DTO
{
	public  class Language : Result
	{
		#region Properties

		[Serialize("Name")]
		public string Name{ get; set; }

		[Serialize("LanguageCode")]
		public string LanguageCode{ get; set; }

		#endregion
		#region Construction

		public Language(string name, string languageCode)
		{
			Name         = name;
			LanguageCode = languageCode;
		}

		public Language()
		{
				
		}

		#endregion
	}
}