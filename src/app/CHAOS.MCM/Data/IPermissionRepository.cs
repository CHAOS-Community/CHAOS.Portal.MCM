using System.Collections.Generic;
using CHAOS.MCM.Data.Dto;

namespace CHAOS.MCM.Data
{
    public interface IPermissionRepository
    {
        IEnumerable<IFolder> GetFolder();
        IEnumerable<IFolderUserJoin> GetFolderUserJoin();
        IEnumerable<IFolderGroupJoin> GetFolderGroupJoin();
    }
}
