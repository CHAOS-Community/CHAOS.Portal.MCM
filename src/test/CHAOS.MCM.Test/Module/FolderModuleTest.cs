using System;
using System.Collections.Generic;
using System.Linq;
using CHAOS.Extensions;
using CHAOS.MCM.Data.Dto;
using CHAOS.MCM.Data.Dto.Standard;
using CHAOS.MCM.Module;
using CHAOS.MCM.Permission;
using CHAOS.Portal.Core;
using CHAOS.Portal.DTO.Standard;
using Chaos.Mcm.Data;
using Moq;
using NUnit.Framework;
using Folder = CHAOS.MCM.Permission.InMemory.Folder;
using FolderPermission = CHAOS.MCM.Permission.FolderPermission;
using IFolder = CHAOS.MCM.Permission.IFolder;

namespace CHAOS.MCM.Test.Module
{
    [TestFixture]
    public class FolderModuleTest
    {
        [Test]
        public void Should_Get_folder_by_ID()
        {
            var callContext       = new Mock<ICallContext>();
            var permissionManager = new Mock<IPermissionManager>();
            var mcmRepository     = new Mock<IMcmRepository>();
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

            callContext.SetupGet(p => p.User).Returns(userInfo);

            folder.Setup(m => m.DoesUserOrGroupHavePermission(userInfo.GUID.ToGuid(), new List<Guid>(), FolderPermission.Read)).Returns(true);
            permissionManager.Setup(m => m.GetFolders(1)).Returns(folder.Object);
            mcmRepository.Setup(m => m.GetFolderInfo(new[] { folderInfo.ID })).Returns(new[] { folderInfo });
            mcmRepository.Setup(m => m.WithConfiguration(null)).Returns(mcmRepository.Object);

            var module = new FolderModule();
            module.Initialize(permissionManager.Object, mcmRepository.Object);

            var result = module.Get(callContext.Object, 1, null, null, null).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(100, result[0].ID);
        }

        [Test]
        public void Should_Get_folders_by_parentID()
        {
            var callContext       = new Mock<ICallContext>();
            var permissionManager = new Mock<IPermissionManager>();
            var mcmRepository     = new Mock<IMcmRepository>();
            var folder            = new Mock<IFolder>();
            var folder2           = new Mock<IFolder>();

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

            callContext.SetupGet(p => p.User).Returns(userInfo);

            folder2.Setup(m => m.DoesUserOrGroupHavePermission(userInfo.GUID.ToGuid(), new List<Guid>(), FolderPermission.Read)).Returns(true);
            folder.Setup(m => m.GetSubFolders()).Returns(new[] {folder2.Object});
            permissionManager.Setup(m => m.GetFolders(folder.Object.ID)).Returns(folder.Object);
            mcmRepository.Setup(m => m.GetFolderInfo(new[] { folder2.Object.ID })).Returns(new[] { folderInfo });
            mcmRepository.Setup(m => m.WithConfiguration(null)).Returns(mcmRepository.Object);

            var module = new FolderModule();
            module.Initialize(permissionManager.Object, mcmRepository.Object);

            var result = module.Get(callContext.Object, null, null, 100, null).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(101, result[0].ID);
        }

        [Test]
        public void Should_Get_folders_by_parentID_permission_through_group()
        {
            var callContext       = new Mock<ICallContext>();
            var permissionManager = new Mock<IPermissionManager>();
            var mcmRepository     = new Mock<IMcmRepository>();
            var folder            = new Mock<IFolder>();
            var folder2           = new Mock<IFolder>();

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

            callContext.SetupGet(p => p.User).Returns(userInfo);

            folder2.Setup(m => m.DoesUserOrGroupHavePermission(userInfo.GUID.ToGuid(), new List<Guid>(), FolderPermission.Read)).Returns(true);
            folder.Setup(m => m.GetSubFolders()).Returns(new[] { folder2.Object });
            permissionManager.Setup(m => m.GetFolders(folder.Object.ID)).Returns(folder.Object);
            mcmRepository.Setup(m => m.GetFolderInfo(new[] { folder2.Object.ID })).Returns(new[] { folderInfo });
            mcmRepository.Setup(m => m.WithConfiguration(null)).Returns(mcmRepository.Object);

            var module = new FolderModule();
            module.Initialize(permissionManager.Object, mcmRepository.Object);

            var result = module.Get(callContext.Object, null, null, 100, null).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(101, result[0].ID);
        }

        [Test]
        public void Should_Get_Users_and_Groups_with_permission_to_a_folder()
        {
            var permissionManager = new Mock<IPermissionManager>();
            var mcmRepository     = new Mock<IMcmRepository>();
            var callContext       = new Mock<ICallContext>();
            var folder            = new Folder();
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

            var module = new FolderModule();
            module.Initialize(permissionManager.Object, mcmRepository.Object);

            permissionManager.Setup(m => m.GetFolders(folder.ID)).Returns(folder);

            var result = module.GetPermission(callContext.Object, folder.ID);

            Assert.AreEqual(1, result.UserPermissions.Count());
            Assert.AreEqual("e1678025-fbc6-4b8a-a566-b5d7d54d4279", result.UserPermissions.First().Guid.ToString());
            Assert.AreEqual(5, (uint) result.UserPermissions.First().Permission);
            Assert.AreEqual(2, (uint) result.GroupPermissions.First().Permission);
        }

        [Test]
        public void Should_set_users_permission_to_folder()
        {
            var permissionManager = new Mock<IPermissionManager>();
            var mcmRepository     = new Mock<IMcmRepository>();
            var callContext       = new Mock<ICallContext>();
            var folder            = new Mock<IFolder>().SetupProperty(p => p.ID, (uint) 100);
            var userGUID = new UUID("8c50786c-e2bf-4014-8694-e964b54cdd2b");
            var userInfo = new UserInfo(new Guid("4336c09e-c8fa-4773-9503-43ad59dbce99"),
                                        new Guid("cb576e41-9e0a-44a0-ab79-753c383b3661"),
                                        1,
                                        "email",
                                        new DateTime(2000, 06, 06),
                                        new DateTime(2010, 06, 06));

            callContext.SetupGet(p => p.User).Returns(userInfo);
            callContext.SetupGet(p => p.Groups).Returns(new Group[0]);
            permissionManager.Setup(m => m.GetFolders(folder.Object.ID)).Returns(folder.Object);
            folder.Setup(m => m.DoesUserOrGroupHavePermission(userInfo.GUID.ToGuid(), new Guid[0], FolderPermission.Read)).Returns(true);
            mcmRepository.Setup(m => m.SetFolderUserJoin(userGUID.ToGuid(), folder.Object.ID, (uint)FolderPermission.Read)).Returns(1);
            mcmRepository.Setup(m => m.WithConfiguration(null)).Returns(mcmRepository.Object);

            var module = new FolderModule();
            module.Initialize(permissionManager.Object, mcmRepository.Object);

            var result = module.SetPermission(callContext.Object, userGUID, null, folder.Object.ID, (uint)FolderPermission.Read);

            Assert.AreEqual(1, result.Value);
        }

        [Test]
        public void Should_set_zero_users_permission_to_folder()
        {
            var permissionManager = new Mock<IPermissionManager>();
            var mcmRepository     = new Mock<IMcmRepository>();
            var callContext       = new Mock<ICallContext>();
            var folder            = new Mock<IFolder>().SetupProperty(p => p.ID, (uint)100);
            var userGUID = new UUID("8c50786c-e2bf-4014-8694-e964b54cdd2b");
            var userInfo = new UserInfo(new Guid("4336c09e-c8fa-4773-9503-43ad59dbce99"),
                                        new Guid("cb576e41-9e0a-44a0-ab79-753c383b3661"),
                                        1,
                                        "email",
                                        new DateTime(2000, 06, 06),
                                        new DateTime(2010, 06, 06));

            callContext.SetupGet(p => p.User).Returns(userInfo);
            callContext.SetupGet(p => p.Groups).Returns(new Group[0]);
            permissionManager.Setup(m => m.GetFolders(folder.Object.ID)).Returns(folder.Object);
            folder.Setup(m => m.DoesUserOrGroupHavePermission(userInfo.GUID.ToGuid(), new Guid[0], FolderPermission.None)).Returns(true);
            mcmRepository.Setup(m => m.SetFolderUserJoin(userGUID.ToGuid(), folder.Object.ID, (uint)FolderPermission.None)).Returns(1);
            mcmRepository.Setup(m => m.WithConfiguration(null)).Returns(mcmRepository.Object);

            var module = new FolderModule();
            module.Initialize(permissionManager.Object, mcmRepository.Object);

            var result = module.SetPermission(callContext.Object, userGUID, null, folder.Object.ID, (uint)FolderPermission.None);

            Assert.AreEqual(1, result.Value);
        }

        [Test]
        public void Should_Delete_Folder()
        {
            var permissionManager = new Mock<IPermissionManager>();
            var mcmRepository     = new Mock<IMcmRepository>();
            var callContext       = new Mock<ICallContext>();
            var folder            = new Mock<IFolder>().SetupProperty(p => p.ID, (uint)100);
            var userInfo          = new UserInfo(new Guid("4336c09e-c8fa-4773-9503-43ad59dbce99"),
                                        new Guid("cb576e41-9e0a-44a0-ab79-753c383b3661"),
                                        1,
                                        "email",
                                        new DateTime(2000, 06, 06),
                                        new DateTime(2010, 06, 06));

            permissionManager.Setup(m => m.GetFolders(folder.Object.ID)).Returns(folder.Object);
            folder.Setup(m => m.DoesUserOrGroupHavePermission(userInfo.GUID.ToGuid(), new Guid[0], FolderPermission.Delete)).Returns(true);
            mcmRepository.Setup(m => m.WithConfiguration(null)).Returns(mcmRepository.Object);
            mcmRepository.Setup(m => m.DeleteFolder(folder.Object.ID)).Returns(1);
            callContext.SetupGet(p => p.User).Returns(userInfo);
            callContext.SetupGet(p => p.Groups).Returns(new Group[0]);

            var module = new FolderModule();
            module.Initialize(permissionManager.Object, mcmRepository.Object);

            var result = module.Delete(callContext.Object, folder.Object.ID);

            Assert.AreEqual(1, result.Value);
        }

        [Test]
        public void Should_Create_Top_Folder()
        {
            var permissionManager = new Mock<IPermissionManager>();
            var mcmRepository     = new Mock<IMcmRepository>();
            var callContext       = new Mock<ICallContext>();
            var folder            = new Mock<IFolder>().SetupProperty(p => p.ID, (uint)100);
            var userInfo          = new UserInfo(new Guid("4336c09e-c8fa-4773-9503-43ad59dbce99"),
                                        new Guid("cb576e41-9e0a-44a0-ab79-753c383b3661"),
                                        1,
                                        "email",
                                        new DateTime(2000, 06, 06),
                                        new DateTime(2010, 06, 06));
            var folderInfo        = new FolderInfo {ID = 1001};
            var subscriptionGUID  = new UUID("cb576e41-9e0a-44a0-ab79-753c383b3661");

            permissionManager.Setup(m => m.GetFolders(folder.Object.ID)).Returns(folder.Object);
            mcmRepository.Setup(m => m.WithConfiguration(null)).Returns(mcmRepository.Object);
            mcmRepository.Setup(m => m.CreateFolder(userInfo.GUID.ToGuid(), subscriptionGUID.ToGuid(), "title", null, 1)).Returns(folderInfo.ID);
            mcmRepository.Setup(m => m.GetFolderInfo(new[] { folderInfo.ID })).Returns(new[] { folderInfo });
            callContext.SetupGet(p => p.User).Returns(userInfo);
            callContext.SetupGet(p => p.Subscriptions).Returns(new[] { new SubscriptionInfo { GUID = subscriptionGUID, Permission = SubscriptionPermission.CreateFolder}, });
            callContext.SetupGet(p => p.Groups).Returns(new Group[0]);

            var module = new FolderModule();
            module.Initialize(permissionManager.Object, mcmRepository.Object);

            var result = module.Create(callContext.Object, subscriptionGUID, "title", null, 1);

            Assert.AreEqual(1001, result.ID);
        }

        [Test]
        public void Should_Create_Sub_Folder()
        {
            var permissionManager = new Mock<IPermissionManager>();
            var mcmRepository     = new Mock<IMcmRepository>();
            var callContext       = new Mock<ICallContext>();
            var folder            = new Mock<IFolder>().SetupProperty(p => p.ID, (uint)100);
            var userInfo          = new UserInfo(new Guid("4336c09e-c8fa-4773-9503-43ad59dbce99"),
                                        new Guid("cb576e41-9e0a-44a0-ab79-753c383b3661"),
                                        1,
                                        "email",
                                        new DateTime(2000, 06, 06),
                                        new DateTime(2010, 06, 06));
            var folderInfo        = new FolderInfo { ID = 1001 };

            permissionManager.Setup(m => m.GetFolders(folder.Object.ID)).Returns(folder.Object);
            folder.Setup(m => m.DoesUserOrGroupHavePermission(userInfo.GUID.ToGuid(), new Guid[0], FolderPermission.Write)).Returns(true);
            mcmRepository.Setup(m => m.WithConfiguration(null)).Returns(mcmRepository.Object);
            mcmRepository.Setup(m => m.CreateFolder(userInfo.GUID.ToGuid(), null, "title", 100, 1)).Returns(folderInfo.ID);
            mcmRepository.Setup(m => m.GetFolderInfo(new[] { folderInfo.ID })).Returns(new[] { folderInfo });
            callContext.SetupGet(p => p.User).Returns(userInfo);
            callContext.SetupGet(p => p.Subscriptions).Returns(new SubscriptionInfo[0]);
            callContext.SetupGet(p => p.Groups).Returns(new Group[0]);

            var module = new FolderModule();
            module.Initialize(permissionManager.Object, mcmRepository.Object);

            var result = module.Create(callContext.Object, null, "title", 100, 1);

            Assert.AreEqual(1001, result.ID);
        }
    }
}