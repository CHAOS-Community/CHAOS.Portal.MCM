using System.Collections.Generic;
using Chaos.Mcm.Data.Dto;

namespace Chaos.Mcm.Data
{
    public interface IPermissionRepository
    {
        IEnumerable<IFolder> GetFolder();
        IEnumerable<IFolderUserJoin> GetFolderUserJoin();
        IEnumerable<IFolderGroupJoin> GetFolderGroupJoin();
    }
}
