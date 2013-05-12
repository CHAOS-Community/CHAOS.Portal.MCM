using CHAOS.Serialization;

namespace Chaos.Mcm.Data.Dto.Standard
{
    using Chaos.Portal.Core.Data.Model;

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