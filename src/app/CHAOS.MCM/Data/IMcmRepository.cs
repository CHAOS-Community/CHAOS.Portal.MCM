using System.Collections.Generic;
using CHAOS.MCM.Data.Dto;

namespace Chaos.Mcm.Data
{
    public interface IMcmRepository
    {
        IMcmRepository WithConfiguration(string connectionString);

        IEnumerable<IFolderUserJoin> GetFolderUserJoin();
        IEnumerable<IFolderGroupJoin> GetFolderGroupJoin();
        IEnumerable<IFolder> GetFolder();
        IEnumerable<IFolderInfo> GetFolderInfo(IEnumerable<uint> ids);
    }
}
