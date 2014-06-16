using System;
using System.Collections.Generic;
using Chaos.Mcm.Data.Dto;
using Chaos.Mcm.Permission;
using Chaos.Mcm.Permission.InMemory;
using Chaos.Portal.Core.Indexing;
using Chaos.Portal.Core.Indexing.View;
using Moq;
using NUnit.Framework;
using FolderPermission = Chaos.Mcm.Permission.FolderPermission;
using IFolder = Chaos.Mcm.Permission.IFolder;

namespace Chaos.Mcm.Test.Extension.v6
{
    using Portal.Core.Data.Model;

    [TestFixture]
    public class ObjectTest : TestBase
    {
        [Test]
        public void Get_WithSingleGuid_ShouldQueryViewWithIdAndFolders()
        {
            var extension  = Make_ObjectV6Extension();
            var objectGuid = new List<Guid>{new Guid("00000000-0000-0000-0000-000000000001")};
            var folder     = new Folder{ID = 1};
            var user       = Make_User();
            PortalApplication.Setup(m => m.Log.Debug(It.IsAny<string>(), null));
            PortalRequest.SetupGet(p => p.User).Returns(user);
            PermissionManager.Setup(m => m.GetFolders(FolderPermission.Read, user.Guid, It.IsAny<IEnumerable<Guid>>())).Returns(new[] { folder });
            ViewManager.Setup(m => m.GetView("Object").Query(It.Is<IQuery>(q => q.Query == "(Id:00000000-0000-0000-0000-000000000001)AND((FolderAncestors:1))"))).Returns(new PagedResult<IResult>(0, 0, new IResult[0]));

            extension.Get(objectGuid);

            ViewManager.VerifyAll();
        }

        [Test]
        public void Get_WithMultipleGuids_ShouldQueryViewWithIdsAndFolders()
        {
            var extension  = Make_ObjectV6Extension();
            var objectGuid = new List<Guid> { new Guid("00000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000003") };
            var folder     = new Folder{ID = 1};
            var user       = Make_User();
            PortalApplication.Setup(m => m.Log.Debug(It.IsAny<string>(), null));
            PortalRequest.SetupGet(p => p.User).Returns(user);
            PermissionManager.Setup(m => m.GetFolders(FolderPermission.Read, user.Guid, It.IsAny<IEnumerable<Guid>>())).Returns(new[] { folder });
            ViewManager.Setup(m => m.GetView("Object").Query(It.Is<IQuery>(q => q.Query == "(Id:00000000-0000-0000-0000-000000000001 00000000-0000-0000-0000-000000000002 00000000-0000-0000-0000-000000000003)AND((FolderAncestors:1))"))).Returns(new PagedResult<IResult>(0, 0, new IResult[0]));

            extension.Get(objectGuid);

            ViewManager.VerifyAll();
        }
        
        [Test]
        public void Get_WithAccessPointGuid_ShouldAddAccessPointToQuery()
        {
            var extension       = Make_ObjectV6Extension();
            var accessPointGuid = new Guid("00000000-0000-0000-0000-000000000001");
            var user            = Make_User();
            PortalApplication.Setup(m => m.Log.Debug(It.IsAny<string>(), null));
            PortalRequest.SetupGet(p => p.User).Returns(user);
            ViewManager.Setup(m => m.GetView("Object").Query(It.Is<IQuery>(q => q.Query == "(*:*)AND(00000000-0000-0000-0000-000000000001_PubStart:[* TO NOW] AND 00000000-0000-0000-0000-000000000001_PubEnd:[NOW TO *])"))).Returns(new PagedResult<IResult>(0,0,new IResult[0]));

            extension.Get(new List<Guid>(), accessPointGuid: accessPointGuid);

            ViewManager.VerifyAll();
        }
        
        [Test]
        public void Get_WithMultipleFolders_OrFoldersInQuery()
        {
            var extension       = Make_ObjectV6Extension();
            var user            = Make_User();
            var folders = new[] { new Folder { ID = 1 }, new Folder { ID = 2 } };
            PortalApplication.Setup(m => m.Log.Debug(It.IsAny<string>(), null));
            PermissionManager.Setup(m => m.GetFolders(FolderPermission.Read, user.Guid, It.IsAny<IEnumerable<Guid>>())).Returns(folders);
            PortalRequest.SetupGet(p => p.User).Returns(user);
            ViewManager.Setup(m => m.GetView("Object").Query(It.IsAny<IQuery>())).Returns(new PagedResult<IResult>(0, 0, new IResult[0]));

            extension.Get(new List<Guid>());

            ViewManager.Verify(m => m.GetView("Object").Query(It.Is<IQuery>(q => q.Query == "(*:*)AND((FolderAncestors:1)(FolderAncestors:2))")));
        }
        
        [Test]
        public void Get_WithFolderFilter_FilterFoldersWithAccess()
        {
            var extension       = Make_ObjectV6Extension();
            var user            = Make_User();
            var folders = new[] { new Folder { ID = 1 }, new Folder { ID = 2 } };
            PortalApplication.Setup(m => m.Log.Debug(It.IsAny<string>(), null));
            PermissionManager.Setup(m => m.GetFolders(FolderPermission.Read, user.Guid, It.IsAny<IEnumerable<Guid>>())).Returns(folders);
            PortalRequest.SetupGet(p => p.User).Returns(user);
            ViewManager.Setup(m => m.GetView("Object").Query(It.IsAny<IQuery>())).Returns(new PagedResult<IResult>(0, 0, new IResult[0]));

            extension.Get(new List<Guid>(), folderId: 1);

            ViewManager.Verify(m => m.GetView("Object").Query(It.Is<IQuery>(q => q.Query == "(*:*)AND((FolderAncestors:1))")));
        }
        
        [Test]
        public void Get_WithAccessPointGuidAndMultipleIds_ShouldAddAccessPointToQuery()
        {
            var extension       = Make_ObjectV6Extension();
            var accessPointGuid = new Guid("00000000-0000-0000-0000-000000000001");
            var objectGuids     = new List<Guid> { new Guid("00000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000003") };
            var user            = Make_User();
            PortalApplication.Setup(m => m.Log.Debug(It.IsAny<string>(), null));
            PortalRequest.SetupGet(p => p.User).Returns(user);
            ViewManager.Setup(m => m.GetView("Object").Query(It.Is<IQuery>(q => q.Query == "(Id:00000000-0000-0000-0000-000000000002 00000000-0000-0000-0000-000000000003)AND(00000000-0000-0000-0000-000000000001_PubStart:[* TO NOW] AND 00000000-0000-0000-0000-000000000001_PubEnd:[NOW TO *])"))).Returns(new PagedResult<IResult>(0, 0, new IResult[0]));

            extension.Get(objectGuids, accessPointGuid: accessPointGuid);

            ViewManager.VerifyAll();
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

            Assert.AreEqual(1, result);
            McmRepository.Verify(m => m.ObjectDelete(expected.Guid));
            ViewManager.Verify(m => m.Delete("Id:" + expected.Guid));
        }

        [Test]
        public void SetPublishSettings_WithPermission_CallMcmRepositoryAndReturnOne()
        {
            var extension = Make_ObjectV6Extension();
            var obj = Make_Object();
            var userInfo = Make_User();
            var accesspoint = MakeAccessPoint();
            var startDate = DateTime.Now;
            var endDate = DateTime.MaxValue;
            PortalRequest.SetupGet(p => p.User).Returns(userInfo);
            McmRepository.Setup(m => m.AccessPointGet(accesspoint.Guid, It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>(), (uint) AccessPointPermission.Write)).Returns(new [] {accesspoint});
            McmRepository.Setup(m => m.ObjectGet(obj.Guid, true, true, true, true, true)).Returns(obj);
            McmRepository.Setup(m => m.AccessPointPublishSettingsSet(accesspoint.Guid, obj.Guid, startDate, endDate)).Returns(1u);

            var result = extension.SetPublishSettings(obj.Guid, accesspoint.Guid, startDate, endDate);

            Assert.AreEqual(1, result);
            ViewManager.Verify(m => m.Index(obj));
        }

        private AccessPoint MakeAccessPoint()
        {
            return new AccessPoint
            {
                Guid = new Guid("00000000-0000-0000-0000-000000000001")
            };
        }

        #region Helpers

        #endregion

    }
}