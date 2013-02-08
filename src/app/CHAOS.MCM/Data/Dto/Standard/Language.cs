using CHAOS.Serialization;
using Chaos.Portal.Data.Dto.Standard;

namespace Chaos.Mcm.Data.Dto.Standard
{
	public  class Language : Result
	{
		#region Properties

		[Serialize]
		public string Name{ get; set; }

		[Serialize]
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