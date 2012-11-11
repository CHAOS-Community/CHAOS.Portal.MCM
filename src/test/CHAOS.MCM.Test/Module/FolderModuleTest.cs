using System;
using System.Collections.Generic;
using System.Linq;
using CHAOS.Extensions;
using CHAOS.MCM.Data;
using CHAOS.MCM.Data.Dto.Standard;
using CHAOS.MCM.Module;
using CHAOS.MCM.Permission;
using CHAOS.MCM.Permission.InMemory;
using CHAOS.Portal.Core;
using CHAOS.Portal.DTO.Standard;
using Chaos.Mcm.Data;
using Moq;
using NUnit.Framework;
using Folder = CHAOS.MCM.Permission.InMemory.Folder;
using FolderPermission = CHAOS.MCM.Permission.FolderPermission;

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
    }
}