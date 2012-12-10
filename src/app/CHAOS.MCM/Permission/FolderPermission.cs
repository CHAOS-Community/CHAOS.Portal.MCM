using System;

namespace CHAOS.MCM.Permission
{
    [Flags]
    public enum FolderPermission : uint
    {
        None                = 0,
        Upload				= 1 << 0,
        Read                = 1 << 1,
        Write               = 1 << 2,
        CreateUpdateObjects = 1 << 3,
        CreateLink          = 1 << 4,
        DeleteObject        = 1 << 5,
        Update				= 1 << 6,
        Delete              = 1 << 7,
        Max					= uint.MaxValue        
    }
}