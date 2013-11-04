namespace Chaos.Mcm.Test.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Chaos.Mcm.Permission.InMemory;
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
        public void Get_WithSingleGuid_ShouldQueryViewWithIdAndFolders()
        {
            var extension  = Make_ObjectV6Extension();
            var objectGuid = new List<Guid>{new Guid("00000000-0000-0000-0000-000000000001")};
            var view       = new Mock<IView>();
            var folder     = new Folder{ID = 1};
            var user       = Make_User();
            ViewManager.Setup(m => m.GetView("Object")).Returns(view.Object);
            PortalRequest.SetupGet(p => p.User).Returns(user);
            PermissionManager.Setup(m => m.GetFolders(FolderPermission.Read, user.Guid, It.IsAny<IEnumerable<Guid>>())).Returns(new[] { folder });

            extension.Get(objectGuid, null, true, true, true, true, true);

            view.Verify(m => m.Query(It.Is<IQuery>(q => q.Query == "(Id:00000000-0000-0000-0000-000000000001)AND(FolderAncestors:1)")));
        }

        [Test]
        public void Get_WithMultipleGuids_ShouldQueryViewWithIdsAndFolders()
        {
            var extension  = Make_ObjectV6Extension();
            var objectGuid = new List<Guid> { new Guid("00000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000003") };
            var view       = new Mock<IView>();
            var folder     = new Folder{ID = 1};
            var user       = Make_User();
            ViewManager.Setup(m => m.GetView("Object")).Returns(view.Object);
            PortalRequest.SetupGet(p => p.User).Returns(user);
            PermissionManager.Setup(m => m.GetFolders(FolderPermission.Read, user.Guid, It.IsAny<IEnumerable<Guid>>())).Returns(new[] { folder });

            extension.Get(objectGuid, null, true, true, true, true, true);

            view.Verify(m => m.Query(It.Is<IQuery>(q => q.Query == "(Id:00000000-0000-0000-0000-000000000001 00000000-0000-0000-0000-000000000002 00000000-0000-0000-0000-000000000003)AND(FolderAncestors:1)")));
        }
        
        [Test]
        public void Get_WithAccessPointGuid_ShouldAddAccessPointToQuery()
        {
            var extension       = Make_ObjectV6Extension();
            var accessPointGuid = new Guid("00000000-0000-0000-0000-000000000001");
            var view            = new Mock<IView>();
            var user            = Make_User();
            ViewManager.Setup(m => m.GetView("Object")).Returns(view.Object);
            PortalRequest.SetupGet(p => p.User).Returns(user);

            extension.Get(new List<Guid>(), accessPointGuid, true, true, true, true, true);

            view.Verify(m => m.Query(It.Is<IQuery>(q => q.Query == "(*:*)AND(00000000-0000-0000-0000-000000000001_PubStart:[* TO NOW] AND 00000000-0000-0000-0000-000000000001_PubEnd:[NOW TO *])")));
        }
        
        [Test]
        public void Get_WithAccessPointGuidAndMultipleIds_ShouldAddAccessPointToQuery()
        {
            var extension       = Make_ObjectV6Extension();
            var accessPointGuid = new Guid("00000000-0000-0000-0000-000000000001");
            var objectGuids     = new List<Guid> { new Guid("00000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000003") };
            var view            = new Mock<IView>();
            var user            = Make_User();
            ViewManager.Setup(m => m.GetView("Object")).Returns(view.Object);
            PortalRequest.SetupGet(p => p.User).Returns(user);

            extension.Get(objectGuids, accessPointGuid, true, true, true, true, true);

            view.Verify(m => m.Query(It.Is<IQuery>(q => q.Query == "(Id:00000000-0000-0000-0000-000000000002 00000000-0000-0000-0000-000000000003)AND(00000000-0000-0000-0000-000000000001_PubStart:[* TO NOW] AND 00000000-0000-0000-0000-000000000001_PubEnd:[NOW TO *])")));
        }

        [Test]
        public void Create_WithPermissionToFolder_CallMcmRepositoryAndReturnTheObject()
        {
            var extension = Make_ObjectV6Extension();
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
            var extension  = Make_ObjectV6Extension();
            var expected   = Make_Object();
            var userInfo   = Make_User();
            PortalRequest.SetupGet(p => p.User).Returns(userInfo);
            PermissionManager.Setup(m => m.DoesUserOrGroupHavePermissionToFolders(userInfo.Guid, new Guid[0], FolderPermission.DeleteObject, It.IsAny<IEnumerable<IFolder>>())).Returns(true);
            McmRepository.Setup(m => m.ObjectGet(expected.Guid, false, false, false, true, false)).Returns(expected);
            McmRepository.Setup(m => m.ObjectDelete(expected.Guid)).Returns(1);

            var result = extension.Delete(expected.Guid);

            Assert.AreEqual(1, result.Value);
            McmRepository.Verify(m => m.ObjectDelete(expected.Guid));
            ViewManager.Verify(m => m.Delete("Id:" + expected.Guid));
        }

        #region Helpers

        #endregion

    }
}