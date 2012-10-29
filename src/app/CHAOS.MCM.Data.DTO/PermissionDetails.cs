using System;
using System.Collections.Generic;
using CHAOS.Serialization;

namespace CHAOS.MCM.Data.DTO
{
    public class PermissionDetails
    {
        #region Properties

        [Serialize]
        public Guid Guid { get; set; }

        [Serialize]
        public uint Permissions { get; set; }

        [Serialize]
        public IEnumerable<Permission> PermissionFlags { get; set; } 

        #endregion
        #region Construction


        public PermissionDetails() : this(0)
        {
            
        }

        public PermissionDetails(uint permissions)
        {
            PermissionFlags = ConvertToPermissions(permissions);
        }

        private static IEnumerable<Permission> ConvertToPermissions(uint permissions)
        {
            var flags = new List<Permission>();

            for (int i = 1, shift = 1 << i; shift < (uint) FolderPermissions.All; i++, shift = 1 << i)
            {
                if ((permissions & shift) == shift)
                    flags.Add(new Permission(((FolderPermissions) shift).ToString(), (uint) shift));
            }

            return flags;
        }

        #endregion
    }
}