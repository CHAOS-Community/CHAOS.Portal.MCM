using System.Collections.Generic;
using CHAOS.Serialization;
using Chaos.Portal.Data.Dto.Standard;

namespace Chaos.Mcm.Data.Dto.Standard
{
    public class FolderPermission : Result
    {
        [Serialize]
        public IEnumerable<EntityPermission> UserPermissions { get; set; }

        [Serialize]
        public IEnumerable<EntityPermission> GroupPermissions { get; set; }

        #region Constructors

        public FolderPermission(IEnumerable<EntityPermission> userPermissions, IEnumerable<EntityPermission> groupPermissions)
        {
            UserPermissions  = userPermissions;
            GroupPermissions = groupPermissions;
        }

        public FolderPermission() : this(new List<EntityPermission>(), new List<EntityPermission>())
        {

        }

        #endregion
    }
}
