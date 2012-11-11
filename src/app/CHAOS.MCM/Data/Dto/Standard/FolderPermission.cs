using System.Collections.Generic;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Serialization;

namespace CHAOS.MCM.Data.Dto.Standard
{
    public class FolderPermission : Result
    {
        [Serialize]
        public IEnumerable<IEntityPermission> UserPermissions { get; set; }

        [Serialize]
        public IEnumerable<IEntityPermission> GroupPermissions { get; set; }

        #region Constructors

        public FolderPermission(IEnumerable<IEntityPermission> userPermissions, IEnumerable<IEntityPermission> groupPermissions)
        {
            UserPermissions = userPermissions;
            GroupPermissions = groupPermissions;
        }

        public FolderPermission() : this(new List<IEntityPermission>(), new List<IEntityPermission>())
        {

        }

        #endregion
    }
}
