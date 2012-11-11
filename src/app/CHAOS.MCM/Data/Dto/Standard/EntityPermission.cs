using System;
using CHAOS.Serialization;

namespace CHAOS.MCM.Data.Dto.Standard
{
    public class EntityPermission : IEntityPermission
    {
        [Serialize("GUID")]
        public Guid Guid { get; set; }
        public MCM.Permission.FolderPermission Permission { get; set; }

        [Serialize("Permission")]
        public uint PermissionUint{ get { return (uint) Permission; }}

        //for( int i = 1, shift = 1 << i; shift < (uint) FolderPermissions.All; i++, shift = 1 << i )
        //{
        //    permissions.Add( new Permission( ( (FolderPermissions) shift).ToString(), (uint) shift ) );
        //}
    }
}