using Geckon.Portal.Data.Result.Standard;
using Geckon.Serialization;

public class FolderType : Result
{
	#region Properties

	[Serialize("ID")]
	public int ID{ get; set; }

	[Serialize("Name")]
	public string Name{ get; set; }

	#endregion
}