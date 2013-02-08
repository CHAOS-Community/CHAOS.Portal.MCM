namespace Chaos.Mcm.Test.Extension
{
    using System;

    using CHAOS;

    using Chaos.Mcm.Extension;
    using Chaos.Portal.Data.Dto.Standard;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class MetadataTest : TestBase
    {
        [Test]
        public void MetadataSet_CallWithValidXml_ShouldCallMetadataSetOnMcmRepository()
        {
            var extension  = Make_MetadataExtension();
            var metadata   = Make_MetadataDto();
            var objectGuid = new Guid("9b8f4e50-1dfd-45ba-b5e8-176bfe8a2fd7");
            CallContext.SetupGet(p => p.User).Returns(new UserInfo { GUID = new UUID("905c48db-5632-4c57-9a1a-a158deba6ab4") });
            
            extension.Metadata_Set(CallContext.Object, objectGuid, metadata.MetadataSchemaGuid, metadata.LanguageCode, metadata.RevisionID, metadata.MetadataXml);

            McmRepository.Verify(m => m.MetadataSet(objectGuid, It.IsAny<Guid>(), metadata.MetadataSchemaGuid, metadata.LanguageCode, metadata.RevisionID, metadata.MetadataXml, It.IsAny<Guid>()));
        }

        [Test]
        public void MetadataSet_CallWithValidXml_ShouldCallObjectGetOnMcmRepository()
        {
            var extension = Make_MetadataExtension();
            var metadata = Make_MetadataDto();
            var objectGuid = new Guid("9b8f4e50-1dfd-45ba-b5e8-176bfe8a2fd7");
            CallContext.SetupGet(p => p.User).Returns(new UserInfo { GUID = new UUID("905c48db-5632-4c57-9a1a-a158deba6ab4") });

            extension.Metadata_Set(CallContext.Object, objectGuid, metadata.MetadataSchemaGuid, metadata.LanguageCode, metadata.RevisionID, metadata.MetadataXml);

            McmRepository.Verify(m => m.GetObject(objectGuid, true, false, false, true, true));
        }
//
//        [Test]
//        public void Set_CallWithValidXml_ShouldCallIndexOnViewManager()
//        {
//            var extension = Make_MetadataExtension();
//            var metadata = Make_MetadataDto();
//            var objectGuid = new Guid("9b8f4e50-1dfd-45ba-b5e8-176bfe8a2fd7");
//            CallContext.SetupGet(p => p.User).Returns(new UserInfo { GUID = new UUID("905c48db-5632-4c57-9a1a-a158deba6ab4") });
//
//            extension.Metadata_Set(CallContext.Object, objectGuid, metadata.MetadataSchemaGuid, metadata.LanguageCode, metadata.RevisionID, metadata.MetadataXml);
//
//            CallContext.Verify(m => m.ViewManager.Index());
//        }

        #region Helpers

        private Metadata Make_MetadataExtension()
        {
            return (Metadata)new Metadata().WithConfiguration(this.PermissionManager.Object, this.McmRepository.Object);
        }

        #endregion

    }
}