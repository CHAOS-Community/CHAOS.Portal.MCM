using System;
using CHAOS.Extensions;
using CHAOS.Serialization;

namespace Chaos.Mcm.Data.Dto.Standard
{
    public class EntityPermission : IEntityPermission
    {
        [Serialize("GUID")]
        public string pGuid{ get { return Guid.ToUUID().ToString(); }}
        
        public Guid Guid { get; set; }
        public Mcm.Permission.FolderPermission Permission { get; set; }

        [Serialize("Permission")]
        public uint PermissionUint{ get { return (uint) Permission; }}
    }
}