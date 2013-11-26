namespace Chaos.Mcm.Extension.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public interface IUserProfileController
    {
        void Set(Guid userGuid, Guid metadataSchemaGuid, XDocument metadata, Guid requestingUsersGuid);
        IList<Data.Dto.UserProfile> Get(Guid userGuid, Guid metadataSchemaGuid);
    }
}