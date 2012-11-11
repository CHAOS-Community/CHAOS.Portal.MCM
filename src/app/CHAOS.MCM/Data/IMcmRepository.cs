using System;
using System.Collections.Generic;
using CHAOS.MCM.Data.Dto;

namespace Chaos.Mcm.Data
{
    public interface IMcmRepository
    {
        IMcmRepository WithConfiguration(string connectionString);

        IEnumerable<IFolderUserJoin> GetFolderUserJoin();
        uint SetFolderUserJoin(Guid userGuid, uint folderID, uint permission);
        IEnumerable<IFolderGroupJoin> GetFolderGroupJoin();
        uint SetFolderGroupJoin(Guid groupGuid, uint folderID, uint permission);
        IEnumerable<IFolder> GetFolder();
        IEnumerable<IFolderInfo> GetFolderInfo(IEnumerable<uint> ids);
    }
}
