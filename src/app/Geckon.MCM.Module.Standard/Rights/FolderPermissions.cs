using System;

namespace Geckon.MCM.Module.Standard.Rights
{
	[Flags]
	public enum FolderPermissions : uint
	{
		None                = 0,
		Read                = 1 << 1,
		Write               = 1 << 2,
		CreateUpdateObjects = 1 << 3,
        CreateLink          = 1 << 4,
		All                 = Read | Write | CreateUpdateObjects | CreateLink
	}
}
