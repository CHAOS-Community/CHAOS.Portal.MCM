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
        uint DeleteFolder(uint id);
        uint CreateFolder(Guid userGuid, Guid? subscriptionGuid, string title, uint? parentID, uint folderTypeID);
        IEnumerable<IFolderInfo> GetFolderInfo(IEnumerable<uint> ids);
        IEnumerable<IAccessPoint> GetAccessPoint(Guid accessPointGuid, Guid userGuid, IEnumerable<Guid> groupGuids, uint permission);
        uint SetAccessPointPublishSettings(Guid accessPointGuid, Guid objectGuid, DateTime? startDate, DateTime? endDate);
        IEnumerable<IObject> GetObject(Guid objectGuid, bool includeMetadata, bool includeFiles, bool includeObjectRelations, bool includeFolders, bool includeAccessPoint);
    }
}
