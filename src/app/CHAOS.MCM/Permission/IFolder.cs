using System;
using System.Collections.Generic;

namespace CHAOS.MCM.Permission
{
    public interface IFolder
    {
        uint ID { get; set; }
        string Name { get; set; }
        DateTime DateCreated { get; set; }
        IDictionary<Guid, FolderPermission> UserPermissions  { get; set; }
        IDictionary<Guid, FolderPermission> GroupPermissions { get; set; }

        IFolder ParentFolder { get; set; }
        IEnumerable<IFolder> GetSubFolders();
        
        void AddSubFolder(IFolder folder);
    }
}