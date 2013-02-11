namespace Chaos.Mcm.Test.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Chaos.Mcm.Data.Dto;
    using Chaos.Portal.Data.Dto.Standard;

    using Moq;

    using NUnit.Framework;

    using FolderPermission = Chaos.Mcm.Permission.FolderPermission;
    using IFolder = Chaos.Mcm.Permission.IFolder;
    using Object = Chaos.Mcm.Data.Dto.Object;

    [TestFixture]
    public class ObjectTest : TestBase
    {
        [Test]
        public void Get_WithSingleGuid_ShouldCallMcmRepositoryWithGuid()
        {
            var extension  = Make_ObjectExtension();
            var objectGuid = new List<Guid>{Guid.NewGuid()};

            extension.Get(CallContext.Object, objectGuid, true, true, true, true, true);

            McmRepository.Verify(m => m.ObjectGet(It.IsAny<IEnumerable<Guid>>(), true, true, true, true, true ));
        }

        [Test]
        public void Get_WithSingleGuid_ShouldReturnObjectRecievedFromRepository()
        {
            var extension  = Make_ObjectExtension();
            var expected   = new Object();
            var objectGuid = new List<Guid> { Guid.NewGuid() };
            McmRepository.Setup(m => m.ObjectGet(It.IsAny<IEnumerable<Guid>>(), false, false, false, false, false)).Returns(new[] { expected });

            var result = extension.Get( CallContext.Object, objectGuid, false, false, false, false, false);

            Assert.AreEqual(expected, result.First());
        }

        [Test]
        public void Create_WithPermissionToFolder_CallMcmRepositoryAndReturnTheObject()
        {
            var extension = Make_ObjectExtension();
            var expected  = Make_Object();
            var folderID  = 1u;
            var userInfo  = new UserInfo { Guid = new Guid("f280d42b-163e-41d3-b0a2-cd59a9ab8fda") };
            var groups    = new Group[0];
            var folder    = new Mock<IFolder>();
            CallContext.SetupGet(p => p.User).Returns(userInfo);
            CallContext.SetupGet(p => p.Groups).Returns(groups);
            PermissionManager.Setup(m => m.GetFolders(folderID)).Returns(folder.Object);
            folder.Setup(m => m.DoesUserOrGroupHavePermission(userInfo.Guid, new Guid[0], FolderPermission.CreateUpdateObjects)).Returns(true);
            McmRepository.Setup(m => m.ObjectGet(expected.Guid, false, false,false,false,false)).Returns(expected);

            var result = extension.Create(CallContext.Object, expected.Guid, expected.ObjectTypeID, folderID);

            Assert.AreEqual(expected, result);
            McmRepository.Verify(m => m.ObjectCreate(expected.Guid, expected.ObjectTypeID, folderID));
        }

        [Test]
        public void Delete_WithPermissions_CallMcmRepositoryAndReturnOne()
        {
            var extension  = Make_ObjectExtension();
            var expected   = Make_Object();
            var userInfo   = new UserInfo { Guid = new Guid("f280d42b-163e-41d3-b0a2-cd59a9ab8fda") };
            var groups     = new Group[0];
            CallContext.SetupGet(p => p.User).Returns(userInfo);
            CallContext.SetupGet(p => p.Groups).Returns(groups);
            PermissionManager.Setup(m => m.DoesUserOrGroupHavePermissionToFolders(userInfo.Guid, new Guid[0], FolderPermission.DeleteObject, It.IsAny<IEnumerable<IFolder>>())).Returns(true);
            McmRepository.Setup(m => m.ObjectGet(expected.Guid, false, false, false, true, false)).Returns(expected);
            McmRepository.Setup(m => m.ObjectDelete(expected.Guid)).Returns(1);

            var result = extension.Delete(CallContext.Object, expected.Guid);

            Assert.AreEqual(1, result.Value);
            McmRepository.Verify(m => m.ObjectDelete(expected.Guid));
        }

        #region Helpers

        private Mcm.Extension.Object Make_ObjectExtension()
        {
            return (Mcm.Extension.Object)new Mcm.Extension.Object().WithConfiguration(PermissionManager.Object, McmRepository.Object);
        }

        #endregion

    }
}