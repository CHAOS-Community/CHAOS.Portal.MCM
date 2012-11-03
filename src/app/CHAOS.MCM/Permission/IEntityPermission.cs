using System;

namespace CHAOS.MCM.Permission
{
    public interface IEntityPermission
    {
        Guid Guid { get; set; }
        FolderPermission Permission { get; set; }

        FolderPermission CombinePermission(FolderPermission with);
    }
}