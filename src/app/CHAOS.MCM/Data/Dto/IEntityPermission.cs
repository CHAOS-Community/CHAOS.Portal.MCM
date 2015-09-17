using System;

namespace Chaos.Mcm.Data.Dto
{
    public interface IEntityPermission
    {
        Guid Guid { get; set; }
        Permission.FolderPermission Permission { get; set; }
    }
}
