namespace Chaos.Mcm.Test.Extension
{
    using Chaos.Mcm.Extension;

    using NUnit.Framework;

    [TestFixture]
    public class LinkTest : TestBase
    {
        [Test]
        public void Create_WithPermission_CallMcmRepositoryReturnOne()
        {
            var extension = Make_LinkExtension();
            var obj       = Make_Object();
            var folder    = Make_FolderInfo();
            McmRepository.Setup(m => m.LinkCreate(obj.Guid, folder.ID, 2)).Returns(1);

            var result = extension.Create(CallContext.Object, obj.Guid, folder.ID);

            Assert.AreEqual(1, result.Value);
            McmRepository.Verify(m => m.LinkCreate(obj.Guid, folder.ID, 2) );
        }

        [Test]
        public void Update_WithPermission_CallMcmRepositoryReturnOne()
        {
            var extension   = Make_LinkExtension();
            var obj         = Make_Object();
            var folder      = Make_FolderInfo();
            var newFolderID = 2u;

            McmRepository.Setup(m => m.LinkUpdate(obj.Guid, folder.ID, newFolderID)).Returns(1);

            var result = extension.Update(CallContext.Object, obj.Guid, folder.ID, newFolderID);

            Assert.AreEqual(1, result.Value);
            McmRepository.Verify(m => m.LinkUpdate(obj.Guid, folder.ID, newFolderID));
        }

        [Test]
        public void Delete_WithPermission_CallMcmRepositoryReturnOne()
        {
            var extension   = Make_LinkExtension();
            var obj         = Make_Object();
            var folder      = Make_FolderInfo();

            McmRepository.Setup(m => m.LinkDelete(obj.Guid, folder.ID)).Returns(1);

            var result = extension.Delete(CallContext.Object, obj.Guid, folder.ID);

            Assert.AreEqual(1, result.Value);
            McmRepository.Verify(m => m.LinkDelete(obj.Guid, folder.ID));
        }

        #region Helpers

        private Link Make_LinkExtension()
        {
            return (Link)new Link().WithConfiguration(this.PermissionManager.Object, this.McmRepository.Object);
        }

        #endregion

    }
}