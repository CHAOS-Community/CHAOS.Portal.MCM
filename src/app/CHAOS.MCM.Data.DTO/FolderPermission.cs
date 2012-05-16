using System.Collections.Generic;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Serialization;

namespace CHAOS.MCM.Data.DTO
{
    public class FolderPermission : Result
    {
        [Serialize]
        public uint AccumulatedPermission { get; set; }

        [Serialize]
        public IEnumerable<Permission> PermissionDetails { get; set; }

        #region Constructors

        public FolderPermission()
        {
            
        }

        public FolderPermission( uint accumulatedPermissions, IEnumerable<Permission> permissionDetails )
        {
            AccumulatedPermission = accumulatedPermissions;
            PermissionDetails     = permissionDetails;
        }

        #endregion
    }
}
