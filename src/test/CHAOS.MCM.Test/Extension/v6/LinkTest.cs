using Chaos.Mcm.Permission;
using NUnit.Framework;

namespace Chaos.Mcm.Test.Extension.v6
{
    using System;
    using System.Collections.Generic;
    using Moq;

    [TestFixture]
    public class LinkTest : TestBase
    {
        [Test]
        public void Create_WithPermission_CallMcmRepositoryReturnOne()
        {
            var extension = Make_LinkExtension();
            var obj       = Make_Object();
            var folder    = Make_FolderInfo();
            PermissionManager.Setup(m => m.HasPermissionToObject(It.IsAny<Guid>(),
                                                                 It.IsAny<Guid>(),
                                                                 It.IsAny<IEnumerable<Guid>>(),
                                                                 FolderPermission.CreateLink)).Returns(true);
            McmRepository.Setup(m => m.LinkCreate(obj.Guid, folder.ID, 2)).Returns(1);

            var result = extension.Create(obj.Guid, folder.ID);

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
            PermissionManager.Setup(m => m.HasPermissionToObject(It.IsAny<Guid>(),
                                                                 It.IsAny<Guid>(),
                                                                 It.IsAny<IEnumerable<Guid>>(),
                                                                 FolderPermission.CreateLink)).Returns(true);
            McmRepository.Setup(m => m.LinkUpdate(obj.Guid, folder.ID, newFolderID)).Returns(1);

            var result = extension.Update(obj.Guid, folder.ID, newFolderID);

            Assert.AreEqual(1, result.Value);
            McmRepository.Verify(m => m.LinkUpdate(obj.Guid, folder.ID, newFolderID));
        }

        [Test]
        public void Delete_WithPermission_CallMcmRepositoryReturnOne()
        {
            var extension   = Make_LinkExtension();
            var obj         = Make_Object();
            var folder      = Make_FolderInfo();
            PermissionManager.Setup(m => m.HasPermissionToObject(It.IsAny<Guid>(),
                                                                 It.IsAny<Guid>(),
                                                                 It.IsAny<IEnumerable<Guid>>(),
                                                                 FolderPermission.CreateLink)).Returns(true);
            McmRepository.Setup(m => m.LinkDelete(obj.Guid, folder.ID)).Returns(1);

            var result = extension.Delete(obj.Guid, folder.ID);

            Assert.AreEqual(1, result.Value);
            McmRepository.Verify(m => m.LinkDelete(obj.Guid, folder.ID));
        }

        #region Helpers

        #endregion

    }
}