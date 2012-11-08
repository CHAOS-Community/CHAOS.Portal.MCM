using System.Collections.Generic;
using CHAOS.MCM.Data.DTO;

namespace CHAOS.MCM.Data
{
    public interface IPermissionRepository
    {
        IEnumerable<Folder> GetFolder();
        IEnumerable<FolderUserJoin> GetFolderUserJoin();
        IEnumerable<FolderGroupJoin> GetFolderGroupJoin();
    }
}
