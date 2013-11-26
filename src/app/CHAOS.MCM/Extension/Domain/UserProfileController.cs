namespace Chaos.Mcm.Extension.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using CHAOS.Extensions;
    using Data;
    using Object = Data.Dto.Object;

    public class UserProfileController : IUserProfileController
    {
        public IMcmRepository McmRepository { get; set; }

        public UserProfileController(IMcmRepository mcmRepository)
        {
            McmRepository = mcmRepository;
        }

        public void Set(Guid userGuid, Guid metadataSchemaGuid, XDocument metadata, Guid requestingUsersGuid)
        {
            var userObject = GetUserProfileObject(userGuid);
            var existingMetadata = userObject.Metadatas.DoIfIsNotNull(ms => ms.FirstOrDefault(m => m.MetadataSchemaGuid == metadataSchemaGuid));
            var metadataGuid = existingMetadata == null ? Guid.NewGuid() : existingMetadata.Guid;
            var revision = existingMetadata == null ? 0 : existingMetadata.RevisionID;

            if (McmRepository.MetadataSet(userObject.Guid, metadataGuid, metadataSchemaGuid, null, revision, metadata, requestingUsersGuid) != 1)
                throw new Exception("Failed to set user profile");
        }

        public IList<Data.Dto.UserProfile> Get(Guid userGuid, Guid metadataSchemaGuid)
        {
            var userObject = McmRepository.ObjectGet(userGuid, true);

            var result = new List<Data.Dto.UserProfile>();

            if (userObject == null || userObject.Metadatas == null)
                return result;

            var metadata = userObject.Metadatas.FirstOrDefault(m => m.MetadataSchemaGuid == metadataSchemaGuid);

            if (metadata != null)
                result.Add(new Data.Dto.UserProfile(metadata));

            return result;
        }

        private Object GetUserProfileObject(Guid userGuid)
        {
            var userObject = McmRepository.ObjectGet(userGuid, true);

            if (userObject == null)
                throw new NotImplementedException("User does not have an user object");

            return userObject;
        }
    }
}
