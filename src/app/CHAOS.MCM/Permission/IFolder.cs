using System;
using System.Collections.Generic;

namespace CHAOS.MCM.Permission
{
    public interface IFolder
    {
        uint ID { get; set; }
        string Name { get; set; }
        DateTime DateCreated { get; set; }
        IDictionary<Guid, IEntityPermission> UserPermissions  { get; set; }
        IDictionary<Guid, IEntityPermission> GroupPermissions { get; set; }

        IFolder ParentFolder { get; set; }
        IEnumerable<IFolder> GetSubFolders();
        
        void AddSubFolder(IFolder folder);
    }
}