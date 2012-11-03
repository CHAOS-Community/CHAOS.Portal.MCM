using System;
using System.Collections.Generic;

namespace CHAOS.MCM.Permission
{
    public interface IFolder
    {
        uint ID { get; set; }
        IFolder Parent { get; set; }
        string Name { get; set; }
        DateTime DateCreated { get; set; }
        IDictionary<Guid, IEntityPermission> UserPermissions  { get; set; }
        IDictionary<Guid, IEntityPermission> GroupPermissions { get; set; }
    }
}