namespace Chaos.Mcm.Data.Dto
{
    using System.Collections.Generic;
    using System.Linq;

    using CHAOS.Serialization;

    using Chaos.Portal.Data.Dto;

    public class FolderPermission : AResult
    {
        [Serialize]
        public uint FolderID { get; set; }

        [Serialize]
        public IList<IEntityPermission> UserPermissions { get; set; }

        [Serialize]
        public IList<IEntityPermission> GroupPermissions { get; set; }

        #region Constructors

        public FolderPermission(IEnumerable<IEntityPermission> userPermissions, IEnumerable<IEntityPermission> groupPermissions)
        {
            this.UserPermissions  = userPermissions.ToList();
            this.GroupPermissions = groupPermissions.ToList();
        }

        public FolderPermission() : this(new List<IEntityPermission>(), new List<IEntityPermission>())
        {

        }

        #endregion
    }
}
