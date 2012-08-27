using System;

namespace CHAOS.MCM.Module.Rights
{
	[Flags]
	public enum FolderPermissions : uint
	{
		None                = 0,
		Upload				= 1 << 0,
		Read                = 1 << 1,
		Write               = 1 << 2,
		CreateUpdateObjects = 1 << 3,
        CreateLink          = 1 << 4,
        DeleteObject        = 1 << 5,
		Update				= 1 << 6,
		All                 = Upload | Read | Write | CreateUpdateObjects | CreateLink | DeleteObject | Update,
		Max					= uint.MaxValue
	}
}
