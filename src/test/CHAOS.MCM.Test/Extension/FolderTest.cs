namespace Chaos.Mcm.Test.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CHAOS;
    using CHAOS.Extensions;

    using Chaos.Mcm.Data.Dto.Standard;
    using Chaos.Mcm.Permission;
    using Chaos.Mcm.Data;
    using Chaos.Portal;
    using Chaos.Portal.Data.Dto.Standard;

    using Moq;

    using NUnit.Framework;

    using Folder = Chaos.Mcm.Permission.InMemory.Folder;
    using FolderPermission = Chaos.Mcm.Permission.FolderPermission;

    [TestFixture]
    public class FolderTest : TestBase
    {
        [Test]
        public void Should_Get_folder_by_ID()
        {
            var folder            = new Mock<IFolder>();

            folder.SetupProperty(p => p.ID, (uint) 100);

            var userInfo = new UserInfo(new Guid("4336c09e-c8fa-4773-9503-43ad59dbce99"),
                                        new Guid("cb576e41-9e0a-44a0-ab79-753c383b3661"),
                                        1,
                                        "email",
                                        new DateTime(2000, 06, 06),
                                        new DateTime(2010, 06, 06));
            var folderInfo = new FolderInfo(100,
                                            1,
                                            null,
                                            new Guid("e9a9581d-1f51-4de2-844e-4088698da28b"),
                                            "folder name",
                                            new DateTime(2005, 05, 05),
                                            0,
                                            6);

            CallContext.SetupGet(p => p.User).Returns(userInfo);

            folder.Setup(m => m.DoesUserOrGroupHavePermission(userInfo.GUID.ToGuid(), new List<Guid>(), FolderPermission.Read)).Returns(true);
            PermissionManager.Setup(m => m.GetFolders(1)).Returns(folder.Object);
            McmRepository.Setup(m => m.GetFolderInfo(new[] { folderInfo.ID })).Returns(new[] { folderInfo });

            var module = new Chaos.Mcm.Extension.Folder();
            module.WithConfiguration(PermissionManager.Object, McmRepository.Object);

            var result = module.Get(CallContext.Object, 1, null, null, null).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(100, result[0].ID);
        }

        [Test]
        public void Should_Get_folders_by_parentID()
        {
            var folder  = new Mock<IFolder>();
            var folder2 = new Mock<IFolder>();

            folder.SetupProperty(p => p.ID, (uint) 100);
            folder2.SetupProperty(p => p.ID, (uint) 101)
                   .SetupProperty(p => p.ParentID, (uint) 100)
                   .SetupProperty(p => p.ParentFolder, folder.Object );

            var userInfo = new UserInfo(new Guid("4336c09e-c8fa-4773-9503-43ad59dbce99"),
                                        new Guid("cb576e41-9e0a-44a0-ab79-753c383b3661"),
                                        1,
                                        "email",
                                        new DateTime(2000, 06, 06),
                                        new DateTime(2010, 06, 06));
            var folderInfo = new FolderInfo(101,
                                            1,
                                            null,
                                            new Guid("e9a9581d-1f51-4de2-844e-4088698da28b"),
                                            "folder name",
                                            new DateTime(2005, 05, 05),
                                            1,
                                            6);

            CallContext.SetupGet(p => p.User).Returns(userInfo);

            folder2.Setup(m => m.DoesUserOrGroupHavePermission(userInfo.GUID.ToGuid(), new List<Guid>(), FolderPermission.Read)).Returns(true);
            folder.Setup(m => m.GetSubFolders()).Returns(new[] {folder2.Object});
            PermissionManager.Setup(m => m.GetFolders(folder.Object.ID)).Returns(folder.Object);
            McmRepository.Setup(m => m.GetFolderInfo(new[] { folder2.Object.ID })).Returns(new[] { folderInfo });

            var module = new Chaos.Mcm.Extension.Folder();
            module.WithConfiguration(PermissionManager.Object, McmRepository.Object);

            var result = module.Get(CallContext.Object, null, null, 100, null).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(101, result[0].ID);
        }

        [Test]
        public void Should_Get_folders_by_parentID_permission_through_group()
        {
            var folder  = new Mock<IFolder>();
            var folder2 = new Mock<IFolder>();

            folder.SetupProperty(p => p.ID, (uint)100);
            folder2.SetupProperty(p => p.ID, (uint)101)
                   .SetupProperty(p => p.ParentID, (uint)100)
                   .SetupProperty(p => p.ParentFolder, folder.Object);

            var userInfo = new UserInfo(new Guid("4336c09e-c8fa-4773-9503-43ad59dbce99"),
                                        new Guid("cb576e41-9e0a-44a0-ab79-753c383b3661"),
                                        1,
                                        "email",
                                        new DateTime(2000, 06, 06),
                                        new DateTime(2010, 06, 06));
            var folderInfo = new FolderInfo(101,
                                            1,
                                            null,
                                            new Guid("e9a9581d-1f51-4de2-844e-4088698da28b"),
                                            "folder name",
                                            new DateTime(2005, 05, 05),
                                            1,
                                            6);

            CallContext.SetupGet(p => p.User).Returns(userInfo);

            folder2.Setup(m => m.DoesUserOrGroupHavePermission(userInfo.GUID.ToGuid(), new List<Guid>(), FolderPermission.Read)).Returns(true);
            folder.Setup(m => m.GetSubFolders()).Returns(new[] { folder2.Object });
            PermissionManager.Setup(m => m.GetFolders(folder.Object.ID)).Returns(folder.Object);
            McmRepository.Setup(m => m.GetFolderInfo(new[] { folder2.Object.ID })).Returns(new[] { folderInfo });

            var module = new Chaos.Mcm.Extension.Folder();
            module.WithConfiguration(PermissionManager.Object, McmRepository.Object);

            var result = module.Get(CallContext.Object, null, null, 100, null).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(101, result[0].ID);
        }

        [Test]
        public void Should_Get_Users_and_Groups_with_permission_to_a_folder()
        {
            var folder = new Folder();
            folder.AddUser(new EntityPermission
                               {
                                   Guid       = new Guid("e1678025-fbc6-4b8a-a566-b5d7d54d4279"),
                                   Permission = (FolderPermission) 5
                               });
            folder.AddGroup(new EntityPermission
                                {
                                    Guid       = new Guid("60627145-18b5-43cd-89c9-25a9c0f878be"),
                                    Permission = (FolderPermission) 2
                                });

            var module = new Chaos.Mcm.Extension.Folder();
            module.WithConfiguration(PermissionManager.Object, McmRepository.Object);

            PermissionManager.Setup(m => m.GetFolders(folder.ID)).Returns(folder);

            var result = module.GetPermission(CallContext.Object, folder.ID);

            Assert.AreEqual(1, result.UserPermissions.Count());
            Assert.AreEqual("e1678025-fbc6-4b8a-a566-b5d7d54d4279", result.UserPermissions.First().Guid.ToString());
            Assert.AreEqual(5, (uint) result.UserPermissions.First().Permission);
            Assert.AreEqual(2, (uint) result.GroupPermissions.First().Permission);
        }

        [Test]
        public void Should_set_users_permission_to_folder()
        {
            var folder   = new Mock<IFolder>().SetupProperty(p => p.ID, (uint) 100);
            var userGUID = new UUID("8c50786c-e2bf-4014-8694-e964b54cdd2b");
            var userInfo = new UserInfo(new Guid("4336c09e-c8fa-4773-9503-43ad59dbce99"),
                                        new Guid("cb576e41-9e0a-44a0-ab79-753c383b3661"),
                                        1,
                                        "email",
                                        new DateTime(2000, 06, 06),
                                        new DateTime(2010, 06, 06));

            CallContext.SetupGet(p => p.User).Returns(userInfo);
            CallContext.SetupGet(p => p.Groups).Returns(new Group[0]);
            PermissionManager.Setup(m => m.GetFolders(folder.Object.ID)).Returns(folder.Object);
            folder.Setup(m => m.DoesUserOrGroupHavePermission(userInfo.GUID.ToGuid(), new Guid[0], FolderPermission.Read)).Returns(true);
            McmRepository.Setup(m => m.SetFolderUserJoin(userGUID.ToGuid(), folder.Object.ID, (uint)FolderPermission.Read)).Returns(1);

            var module = new Chaos.Mcm.Extension.Folder();
            module.WithConfiguration(PermissionManager.Object, McmRepository.Object);

            var result = module.SetPermission(CallContext.Object, userGUID, null, folder.Object.ID, (uint)FolderPermission.Read);

            Assert.AreEqual(1, result.Value);
        }

        [Test]
        public void Should_set_zero_users_permission_to_folder()
        {
            var folder   = new Mock<IFolder>().SetupProperty(p => p.ID, (uint)100);
            var userGUID = new UUID("8c50786c-e2bf-4014-8694-e964b54cdd2b");
            var userInfo = new UserInfo(new Guid("4336c09e-c8fa-4773-9503-43ad59dbce99"),
                                        new Guid("cb576e41-9e0a-44a0-ab79-753c383b3661"),
                                        1,
                                        "email",
                                        new DateTime(2000, 06, 06),
                                        new DateTime(2010, 06, 06));

            CallContext.SetupGet(p => p.User).Returns(userInfo);
            CallContext.SetupGet(p => p.Groups).Returns(new Group[0]);
            PermissionManager.Setup(m => m.GetFolders(folder.Object.ID)).Returns(folder.Object);
            folder.Setup(m => m.DoesUserOrGroupHavePermission(userInfo.GUID.ToGuid(), new Guid[0], FolderPermission.None)).Returns(true);
            McmRepository.Setup(m => m.SetFolderUserJoin(userGUID.ToGuid(), folder.Object.ID, (uint)FolderPermission.None)).Returns(1);

            var module = new Chaos.Mcm.Extension.Folder();
            module.WithConfiguration(PermissionManager.Object, McmRepository.Object);

            var result = module.SetPermission(CallContext.Object, userGUID, null, folder.Object.ID, (uint)FolderPermission.None);

            Assert.AreEqual(1, result.Value);
        }

        [Test]
        public void Should_Delete_Folder()
        {
            var folder   = new Mock<IFolder>().SetupProperty(p => p.ID, (uint)100);
            var userInfo = new UserInfo(new Guid("4336c09e-c8fa-4773-9503-43ad59dbce99"),
                                        new Guid("cb576e41-9e0a-44a0-ab79-753c383b3661"),
                                        1,
                                        "email",
                                        new DateTime(2000, 06, 06),
                                        new DateTime(2010, 06, 06));

            PermissionManager.Setup(m => m.GetFolders(folder.Object.ID)).Returns(folder.Object);
            folder.Setup(m => m.DoesUserOrGroupHavePermission(userInfo.GUID.ToGuid(), new Guid[0], FolderPermission.Delete)).Returns(true);
            McmRepository.Setup(m => m.DeleteFolder(folder.Object.ID)).Returns(1);
            CallContext.SetupGet(p => p.User).Returns(userInfo);
            CallContext.SetupGet(p => p.Groups).Returns(new Group[0]);

            var module = new Chaos.Mcm.Extension.Folder();
            module.WithConfiguration(PermissionManager.Object, McmRepository.Object);

            var result = module.Delete(CallContext.Object, folder.Object.ID);

            Assert.AreEqual(1, result.Value);
        }

        [Test]
        public void Should_Create_Top_Folder()
        {
            var folder            = new Mock<IFolder>().SetupProperty(p => p.ID, (uint)100);
            var userInfo          = new UserInfo(new Guid("4336c09e-c8fa-4773-9503-43ad59dbce99"),
                                        new Guid("cb576e41-9e0a-44a0-ab79-753c383b3661"),
                                        1,
                                        "email",
                                        new DateTime(2000, 06, 06),
                                        new DateTime(2010, 06, 06));
            var folderInfo        = new FolderInfo {ID = 1001};
            var subscriptionGUID  = new UUID("cb576e41-9e0a-44a0-ab79-753c383b3661");

            PermissionManager.Setup(m => m.GetFolders(folder.Object.ID)).Returns(folder.Object);
            McmRepository.Setup(m => m.CreateFolder(userInfo.GUID.ToGuid(), subscriptionGUID.ToGuid(), "title", null, 1)).Returns(folderInfo.ID);
            McmRepository.Setup(m => m.GetFolderInfo(new[] { folderInfo.ID })).Returns(new[] { folderInfo });
            CallContext.SetupGet(p => p.User).Returns(userInfo);
            CallContext.SetupGet(p => p.Subscriptions).Returns(new[] { new SubscriptionInfo { GUID = subscriptionGUID, Permission = SubscriptionPermission.CreateFolder}, });
            CallContext.SetupGet(p => p.Groups).Returns(new Group[0]);

            var module = new Chaos.Mcm.Extension.Folder();
            module.WithConfiguration(PermissionManager.Object, McmRepository.Object);

            var result = module.Create(CallContext.Object, subscriptionGUID, "title", null, 1);

            Assert.AreEqual(1001, result.ID);
        }

        [Test]
        public void Should_Create_Sub_Folder()
        {
            var folder            = new Mock<IFolder>().SetupProperty(p => p.ID, (uint)100);
            var userInfo          = new UserInfo(new Guid("4336c09e-c8fa-4773-9503-43ad59dbce99"),
                                        new Guid("cb576e41-9e0a-44a0-ab79-753c383b3661"),
                                        1,
                                        "email",
                                        new DateTime(2000, 06, 06),
                                        new DateTime(2010, 06, 06));
            var folderInfo        = new FolderInfo { ID = 1001 };

            PermissionManager.Setup(m => m.GetFolders(folder.Object.ID)).Returns(folder.Object);
            folder.Setup(m => m.DoesUserOrGroupHavePermission(userInfo.GUID.ToGuid(), new Guid[0], FolderPermission.Write)).Returns(true);
            McmRepository.Setup(m => m.CreateFolder(userInfo.GUID.ToGuid(), null, "title", 100, 1)).Returns(folderInfo.ID);
            McmRepository.Setup(m => m.GetFolderInfo(new[] { folderInfo.ID })).Returns(new[] { folderInfo });
            CallContext.SetupGet(p => p.User).Returns(userInfo);
            CallContext.SetupGet(p => p.Subscriptions).Returns(new SubscriptionInfo[0]);
            CallContext.SetupGet(p => p.Groups).Returns(new Group[0]);

            var module = new Chaos.Mcm.Extension.Folder();
            module.WithConfiguration(PermissionManager.Object, McmRepository.Object);

            var result = module.Create(CallContext.Object, null, "title", 100, 1);

            Assert.AreEqual(1001, result.ID);
        }
    }
}