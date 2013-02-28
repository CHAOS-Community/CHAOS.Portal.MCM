using CHAOS.Serialization;
using Chaos.Portal.Data.Dto;

namespace Chaos.Mcm.Data.Dto.Standard
{
	public  class Language : AResult
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