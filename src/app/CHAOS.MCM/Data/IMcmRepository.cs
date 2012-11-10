using System.Collections.Generic;
using CHAOS.MCM.Data.DTO;

namespace CHAOS.MCM.Data
{
    public interface IMcmRepository
    {
        IMcmRepository WithConfiguration(string connectionString);

        IEnumerable<FolderUserJoin> GetFolderUserJoin();
        IEnumerable<FolderGroupJoin> GetFolderGroupJoin();
        IEnumerable<Folder> GetFolder();
        IEnumerable<FolderInfo> GetFolderInfo(IEnumerable<uint> ids);
    }
}
