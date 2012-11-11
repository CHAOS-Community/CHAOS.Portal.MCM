using System;

namespace CHAOS.MCM.Data.Dto
{
    public interface IEntityPermission
    {
        Guid Guid { get; set; }
        Permission.FolderPermission Permission { get; set; }
    }
}
