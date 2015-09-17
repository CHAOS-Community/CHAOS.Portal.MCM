namespace Chaos.Mcm.Data.Dto
{
    using System;

    using CHAOS.Serialization;

    public class EntityPermission : IEntityPermission
    {
        [Serialize("Guid")]
        public Guid Guid { get; set; }

        public Mcm.Permission.FolderPermission Permission { get; set; }

        [Serialize("Permission")]
        public uint PermissionUint{ get { return (uint) this.Permission; }}
    }
}