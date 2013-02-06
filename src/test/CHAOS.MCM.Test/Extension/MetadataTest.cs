namespace Chaos.Mcm.Test.Extension
{
    using System;

    using Chaos.Mcm.Extension;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class MetadataTest : TestBase
    {
//        [Test]
//        public void MetadataSet_CallWithValidXml_ShouldCallMetadataSetOnMcmRepository()
//        {
//            var extension  = Make_MetadataExtension();
//            var metadata   = Make_MetadataDto();
//            var objectGuid = new Guid("9b8f4e50-1dfd-45ba-b5e8-176bfe8a2fd7");
//
//            var result = extension.Metadata_Set(CallContext.Object, objectGuid, metadata.MetadataSchemaGuid, metadata.LanguageCode, metadata.RevisionID, metadata.MetadataXml);
//
//            McmRepository.Verify(m => m.MetadataSet(objectGuid, metadata,It.IsAny<Guid>()));
//            Assert.AreEqual(1, result.Value);
//        }

        #region Helpers

        private Metadata Make_MetadataExtension()
        {
            return (Metadata)new Metadata().WithConfiguration(this.PermissionManager.Object, this.McmRepository.Object);
        }

        #endregion

    }
}