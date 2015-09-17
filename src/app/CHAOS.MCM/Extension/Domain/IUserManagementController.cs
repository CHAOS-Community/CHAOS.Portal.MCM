namespace Chaos.Mcm.Extension.Domain
{
    using System;
    using System.Collections.Generic;

    public interface IUserManagementController
    {
        IList<Data.Dto.Object> GetUserObject(Guid userGuid, Guid requestingUsersGuid, bool createIfMissing = true, bool includeMetadata = false, bool includeFiles = false);
        IList<Data.Dto.Standard.Folder> GetUserFolder(Guid userGuid, Guid requestingUsersGuid, bool createIfMissing = true);
    }
}