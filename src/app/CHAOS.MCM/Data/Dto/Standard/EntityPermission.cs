using System;

namespace CHAOS.MCM.Data.Dto.Standard
{
    public class EntityPermission : IEntityPermission
    {
        public Guid Guid { get; set; }
        public MCM.Permission.FolderPermission Permission { get; set; }
    }
}