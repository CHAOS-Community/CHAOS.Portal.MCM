using System;
using CHAOS.Extensions;
using CHAOS.Serialization;

namespace CHAOS.MCM.Data.Dto.Standard
{
    public class EntityPermission : IEntityPermission
    {
        [Serialize("GUID")]
        public string pGuid{ get { return Guid.ToUUID().ToString(); }}
        
        public Guid Guid { get; set; }
        public MCM.Permission.FolderPermission Permission { get; set; }

        [Serialize("Permission")]
        public uint PermissionUint{ get { return (uint) Permission; }}
    }
}