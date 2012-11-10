using System;

namespace CHAOS.MCM.Permission.InMemory
{
    public class EntityPermission : IEntityPermission
    {
        public Guid Guid { get; set; }
        public FolderPermission Permission { get; set; }

        public FolderPermission CombinePermission(FolderPermission with)
        {
            return Permission = Permission | with;
        }
    }
}