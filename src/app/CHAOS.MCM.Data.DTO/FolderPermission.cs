using System.Collections.Generic;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Serialization;

namespace CHAOS.MCM.Data.DTO
{
    public class FolderPermission : Result
    {
        [Serialize]
        public IEnumerable<PermissionDetails> UserPermissions { get; set; }

        [Serialize]
        public IEnumerable<PermissionDetails> GroupPermissions { get; set; }

        #region Constructors

        public FolderPermission(IEnumerable<PermissionDetails> userPermissions, IEnumerable<PermissionDetails> groupPermissions)
        {
            UserPermissions  = userPermissions;
            GroupPermissions = groupPermissions;
        }

        public FolderPermission() : this(new List<PermissionDetails>(), new List<PermissionDetails>())
        {
            
        }

        #endregion
    }
}
