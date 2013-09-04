namespace Chaos.Mcm.Test.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Indexing;
    using Chaos.Portal.Core.Indexing.View;

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

            extension.Get(objectGuid, null, true, true, true, true, true);

            McmRepository.Verify(m => m.ObjectGet(It.IsAny<IEnumerable<Guid>>(), true, true, true, true, true ));
        }
        
        [Test]
        public void Get_WithAccessPointGuid_ShouldAddAccessPointToQuery()
        {
            var extension       = Make_ObjectExtension();
            var accessPointGuid = new Guid("00000000-0000-0000-0000-000000000000");
            var view            = new Mock<IView>();
            ViewManager.Setup(m => m.GetView("Object")).Returns(view.Object);

            extension.Get(new List<Guid>(), accessPointGuid, true, true, true, true, true);

            view.Verify(m => m.Query(It.Is<IQuery>(q => q.Query == "(*:*)AND(00000000-0000-0000-0000-000000000000_PubStart:[*+TO+NOW]+AND+00000000-0000-0000-0000-000000000000_PubEnd:[NOW+TO+*])")));
        }

        [Test]
        public void Get_WithSingleGuid_ShouldReturnObjectRecievedFromRepository()
        {
            var extension  = Make_ObjectExtension();
            var expected   = new Object();
            var objectGuid = new List<Guid> { Guid.NewGuid() };
            McmRepository.Setup(m => m.ObjectGet(It.IsAny<IEnumerable<Guid>>(), false, false, false, false, false)).Returns(new[] { expected });

            var result = extension.Get(objectGuid, null, false, false, false, false, false);

            Assert.AreEqual(expected, result.Results.First());
        }

        [Test]
        public void Create_WithPermissionToFolder_CallMcmRepositoryAndReturnTheObject()
        {
            var extension = Make_ObjectExtension();
            var expected  = Make_Object();
            var folderID  = 1u;
            var userInfo  = Make_User();
            var folder    = new Mock<IFolder>();
            PortalRequest.SetupGet(p => p.User).Returns(userInfo);
            PermissionManager.Setup(m => m.GetFolders(folderID)).Returns(folder.Object);
            folder.Setup(m => m.DoesUserOrGroupHavePermission(userInfo.Guid, new Guid[0], FolderPermission.CreateUpdateObjects)).Returns(true);
            McmRepository.Setup(m => m.ObjectGet(expected.Guid, false, false,false,false,false)).Returns(expected);

            var result = extension.Create(expected.Guid, expected.ObjectTypeID, folderID);

            Assert.AreEqual(expected, result);
            McmRepository.Verify(m => m.ObjectCreate(expected.Guid, expected.ObjectTypeID, folderID));
        }

        [Test]
        public void Delete_WithPermissions_CallMcmRepositoryAndReturnOne()
        {
            var extension  = Make_ObjectExtension();
            var expected   = Make_Object();
            var userInfo   = Make_User();
            PortalRequest.SetupGet(p => p.User).Returns(userInfo);
            PermissionManager.Setup(m => m.DoesUserOrGroupHavePermissionToFolders(userInfo.Guid, new Guid[0], FolderPermission.DeleteObject, It.IsAny<IEnumerable<IFolder>>())).Returns(true);
            McmRepository.Setup(m => m.ObjectGet(expected.Guid, false, false, false, true, false)).Returns(expected);
            McmRepository.Setup(m => m.ObjectDelete(expected.Guid)).Returns(1);

            var result = extension.Delete(expected.Guid);

            Assert.AreEqual(1, result.Value);
            McmRepository.Verify(m => m.ObjectDelete(expected.Guid));
        }

        #region Helpers

        #endregion

    }
}