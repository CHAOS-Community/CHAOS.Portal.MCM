using System;
using Chaos.Mcm.Permission;
using Moq;
using NUnit.Framework;

namespace Chaos.Mcm.Test.Extension.v6
{
    using System.Collections.Generic;

    [TestFixture]
    public class MetadataTest : TestBase
    {
        [Test]
        public void MetadataSet_CallWithValidXml_ShouldCallMetadataSetOnMcmRepository()
        {
            var extension  = Make_MetadataExtension();
            var metadata   = Make_MetadataDto();
            var objectGuid = new Guid("9b8f4e50-1dfd-45ba-b5e8-176bfe8a2fd7");
            PortalRequest.SetupGet(p => p.User).Returns(Make_User());
            PermissionManager.Setup(m => m.HasPermissionToObject(It.IsAny<Guid>(),
                                                     It.IsAny<Guid>(),
                                                     It.IsAny<IEnumerable<Guid>>(),
                                                     FolderPermission.CreateUpdateObjects)).Returns(true);

            extension.Set(objectGuid, metadata.MetadataSchemaGuid, metadata.LanguageCode, metadata.RevisionID, metadata.MetadataXml);

            McmRepository.Verify(m => m.MetadataSet(objectGuid, It.IsAny<Guid>(), metadata.MetadataSchemaGuid, metadata.LanguageCode, metadata.RevisionID, metadata.MetadataXml, It.IsAny<Guid>()));
        }

        [Test]
        public void MetadataSet_CallWithValidXml_ShouldCallObjectGetOnMcmRepository()
        {
            var extension = Make_MetadataExtension();
            var metadata = Make_MetadataDto();
            var objectGuid = new Guid("9b8f4e50-1dfd-45ba-b5e8-176bfe8a2fd7");
            PermissionManager.Setup(m => m.HasPermissionToObject(It.IsAny<Guid>(),
                                                     It.IsAny<Guid>(),
                                                     It.IsAny<IEnumerable<Guid>>(),
                                                     FolderPermission.CreateUpdateObjects)).Returns(true);

            extension.Set(objectGuid, metadata.MetadataSchemaGuid, metadata.LanguageCode, metadata.RevisionID, metadata.MetadataXml);

            McmRepository.Verify(m => m.ObjectGet(objectGuid, true, false, false, true, true));
        }

        #region Helpers

        #endregion

    }
}