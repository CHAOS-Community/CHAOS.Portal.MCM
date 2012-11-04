using System;
using System.Collections.Generic;

namespace CHAOS.MCM.Permission.InMemory
{
    public class Folder : IFolder
    {
        #region Properties

        public uint ID { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public IDictionary<Guid, IEntityPermission> UserPermissions { get; set; }
        public IDictionary<Guid, IEntityPermission> GroupPermissions { get; set; }
        public IFolder ParentFolder { get; set; }

        #endregion
        #region Business Logic

        public IEnumerable<IFolder> GetSubFolders()
        {
            throw new NotImplementedException();
        }

        public void AddSubFolder(IFolder folder)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
