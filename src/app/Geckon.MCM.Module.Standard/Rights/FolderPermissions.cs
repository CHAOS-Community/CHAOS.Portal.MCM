using System;

namespace Geckon.MCM.Module.Standard.Rights
{
	[Flags]
	public enum FolderPermissions : int
	{
		None  = 0,
		Read  = 1 << 1,
		Write = 1 << 2,
		All   = Read | Write
	}
}
