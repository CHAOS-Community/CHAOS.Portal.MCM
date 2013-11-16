using System;
using System.Linq;
using Chaos.Mcm.Data;
using Chaos.Mcm.Data.Dto;
using Chaos.Mcm.Permission;
using Chaos.Mcm.Permission.InMemory;
using Moq;
using NUnit.Framework;
using Folder = Chaos.Mcm.Permission.InMemory.Folder;
using FolderPermission = Chaos.Mcm.Permission.FolderPermission;
using IFolder = Chaos.Mcm.Permission.IFolder;

namespace Chaos.Mcm.Test.Permission
{
    using System.Collections.Generic;
    using Amazon.SimpleNotificationService.Model;

    [TestFixture]
    public class InMemoryPermissionManagerTest : TestBase
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void Should_add_folder_to_manager()
        {
            var permissionManager = new InMemoryPermissionManager();
            var folder = new Mock<IFolder>();

            permissionManager.AddFolder(folder.Object);
        }

        [Test]
        public void Should_get_folder_by_ids()
        {
            var permissionManager = new InMemoryPermissionManager();
            var folder1 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)1)
                                             .SetupProperty(f => f.Name, "folder 1");
            var folder2 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)2)
                                             .SetupProperty(f => f.Name, "folder 2");

            permissionManager.AddFolder(folder1.Object);
            permissionManager.AddFolder(folder2.Object);

            Assert.AreEqual("folder 1", permissionManager.GetFolders(1).Name);
            Assert.AreEqual("folder 2", permissionManager.GetFolders(2).Name);
            Assert.AreEqual(1, permissionManager.GetFolders(1).ID);
            Assert.AreEqual(2, permissionManager.GetFolders(2).ID);
        }

        [Test]
        public void Should_Add_User_to_folder_with_permission()
        {
            var permissionManager = new InMemoryPermissionManager();
            var folder1 = new Folder{ID = 1};
            var user    = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);
            
            permissionManager.AddFolder(folder1);
            permissionManager.GetFolders(folder1.ID).AddUser(user.Object);
            
            Assert.AreEqual(1, folder1.UserPermissions.Count);
        }
        
        [Test]
        public void Should_assocociate_inherit_userpermissions_when_adding_subfolder()
        {
            var permissionManager = new InMemoryPermissionManager();
            var folder1 = new Folder { ID = 1, Name = "folder 1" };
            var folder2 = new Folder { ID = 2, Name = "folder 2", ParentFolder = folder1};
            var user    = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);

            permissionManager.AddFolder(folder1);
            permissionManager.GetFolders(folder1.ID).AddUser(user.Object);
            permissionManager.AddFolder(folder2);

            var result = permissionManager.GetFolders(folder2.ID);

            Assert.AreEqual("folder 2", result.Name);
            Assert.AreEqual(1, result.UserPermissions.Count);
            Assert.AreEqual("39f26c89-5e6c-46d5-af3a-bc14a7e1486b", result.UserPermissions.First().Key.ToString());          
        }

        [Test]
        public void Should_assocociate_subfolders_with_parent_on_addfolder()
        {
            var permissionManager = new InMemoryPermissionManager();
            var folder1 = new Folder {ID = 1};
            var folder2 = new Folder {ID = 2, ParentFolder = folder1};

            permissionManager.AddFolder(folder1);
            permissionManager.AddFolder(folder2);

            Assert.AreEqual(1, folder1.GetSubFolders().Count());
        }

        [Test]
        public void Should_propagate_userpermissions_to_subfolders_when_adding_a_user()
        {
            var permissionManager = new InMemoryPermissionManager();
            var folder1 = new Folder { ID = 1 };
            var folder2 = new Folder { ID = 1, ParentFolder = folder1 };
            var user    = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);

            permissionManager.AddFolder(folder1);
            permissionManager.AddFolder(folder2);
            permissionManager.GetFolders(folder1.ID).AddUser(user.Object);

            Assert.AreEqual(user.Object.Guid, folder2.UserPermissions.First().Key);
            Assert.AreEqual(user.Object.Permission, folder2.UserPermissions.First().Value);
        }

        [Test]
        public void Should_propagate_grouppermissions_to_subfolders_when_adding_a_group()
        {
            var permissionManager = new InMemoryPermissionManager();
            var folder1 = new Folder { ID = 1 };
            var folder2 = new Folder { ID = 2, ParentFolder = folder1 };
            var folder3 = new Folder { ID = 3, ParentFolder = folder2 };
            var group   = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);

            permissionManager.AddFolder(folder1);
            permissionManager.AddFolder(folder2);
            permissionManager.AddFolder(folder3);
            permissionManager.GetFolders(folder1.ID).AddGroup(group.Object);

            Assert.AreEqual(group.Object.Guid, folder3.GroupPermissions.First().Key);
            Assert.AreEqual(group.Object.Permission, folder3.GroupPermissions.First().Value);
        }

        [Test]
        public void Should_Get_TopFolders()
        {
            var permissionManager = new InMemoryPermissionManager();
            var folder1 = new Folder { ID = 1 };
            var folder2 = new Folder { ID = 2, ParentFolder = folder1 };
            var folder3 = new Folder { ID = 3, ParentFolder = folder2 };
            var folder4 = new Folder { ID = 4 };
            var perm1   = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);
            var perm2   = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("40f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Read);
            
            permissionManager.AddFolder(folder1);
            permissionManager.AddFolder(folder2);
            permissionManager.AddFolder(folder3);
            permissionManager.AddFolder(folder4);
            permissionManager.GetFolders(folder2.ID).AddGroup(perm2.Object);
            permissionManager.GetFolders(folder3.ID).AddUser(perm1.Object);
            permissionManager.GetFolders(folder4.ID).AddUser(perm1.Object);

            var topFolders = permissionManager.GetFolders(FolderPermission.CreateUpdateObjects, perm1.Object.Guid, new[] { perm2.Object.Guid }).ToList();

            Assert.AreEqual(2, topFolders.Count());
            Assert.AreEqual(3, topFolders[0].ID);
            Assert.AreEqual(4, topFolders[1].ID);
        }

        [Test]
        public void HasPermissionToObject_GivenObjectGuidWithPermission_ReturnTrue()
        {
            var permissionManager = new InMemoryPermissionManager(McmRepository.Object);
            var objectGuid = new Guid("af40f5e3-2cbf-944f-b833-6c444ad760e1");
            var userGuid = new Guid("10000000-0000-0000-0000-000000000001");
            var folderDtos = new[] { new Data.Dto.Standard.Folder{ ID = 1 } };
            McmRepository.Setup(m => m.FolderGet(null, null, objectGuid)).Returns(folderDtos);
            permissionManager.AddFolder(new Folder
            {
                ID = 1,
                UserPermissions = new Dictionary<Guid, FolderPermission> { { userGuid, FolderPermission.Read } }
            });

            var result = permissionManager.HasPermissionToObject(objectGuid, userGuid, null, FolderPermission.Read);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Should_Get_TopFolders_With_Group_Permission()
        {
            var permissionManager = new InMemoryPermissionManager();
            var folder1 = new Folder { ID = 1 };
            var folder2 = new Folder { ID = 2 };
            var folder3 = new Folder { ID = 3, ParentFolder = folder1 };
            var folder4 = new Folder { ID = 4, ParentFolder = folder2 };
            var folder5 = new Folder { ID = 5, ParentFolder = folder3};
            var folder6 = new Folder { ID = 6, ParentFolder = folder4};
            var perm1   = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);
            var perm2   = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("40f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);

            permissionManager.AddFolder(folder1);
            permissionManager.AddFolder(folder2);
            permissionManager.AddFolder(folder3);
            permissionManager.AddFolder(folder4);
            permissionManager.AddFolder(folder5);
            permissionManager.AddFolder(folder6);
            permissionManager.GetFolders(folder1.ID).AddGroup(perm1.Object);
            permissionManager.GetFolders(folder4.ID).AddUser(perm1.Object);

            var topFolders = permissionManager.GetFolders(FolderPermission.Read, perm1.Object.Guid, new[] { perm2.Object.Guid, perm1.Object.Guid }).ToList();

            Assert.AreEqual(2, topFolders.Count());
            Assert.AreEqual(folder1.ID, topFolders[0].ID);
            Assert.AreEqual(folder4.ID, topFolders[1].ID);
        }

        [Test]
        public void Should_Get_SubFolders_With_Permission_Through_Group()
        {
            var permissionManager = new InMemoryPermissionManager();
            var folder1 = new Folder { ID = 1 };
            var folder2 = new Folder { ID = 2, ParentFolder = folder1 };
            var folder3 = new Folder { ID = 3, ParentFolder = folder2 };
            var folder4 = new Folder { ID = 4 };
            var perm1   = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);
            var perm2   = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("40f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Read);

            permissionManager.AddFolder(folder1);
            permissionManager.AddFolder(folder2);
            permissionManager.AddFolder(folder3);
            permissionManager.AddFolder(folder4);
            permissionManager.GetFolders(folder1.ID).AddGroup(perm2.Object);

            var hasPermissions = permissionManager.GetFolders(folder1.ID).DoesUserOrGroupHavePermission(perm1.Object.Guid, new[] { perm2.Object.Guid }, FolderPermission.Read );
            var folders        = permissionManager.GetFolders(folder1.ID).GetSubFolders().Where(item => item.DoesUserOrGroupHavePermission(perm1.Object.Guid, new[] { perm2.Object.Guid }, FolderPermission.Read));

            Assert.IsTrue(hasPermissions);
            Assert.AreEqual(2, folders.First().ID);
        }

        [Test]
        public void Should_synchronize_with_permission_repository()
        {
            var permissionManager = new InMemoryPermissionManager();
            var entity = new Mock<IEntityPermission>();
            var mcmRepository = new Mock<IMcmRepository>();
            var syncSpecification = new Mock<ISynchronizationSpecification>();
            entity.SetupProperty(p => p.Guid, new Guid("c86a1cfc-10de-4a51-8d7b-6ba8f985e273"))
                  .SetupProperty(p => p.Permission, FolderPermission.Read);
            mcmRepository.Setup(repo => repo.FolderGet(null, null, null)).Returns(new[] { new Data.Dto.Standard.Folder { ID = 1 } });
            mcmRepository.Setup(repo => repo.FolderPermissionGet()).Returns(new[] { new Data.Dto.FolderPermission { FolderID = 1, UserPermissions = new List<IEntityPermission> { entity.Object }, GroupPermissions = new List<IEntityPermission> { entity.Object } } });

            permissionManager.WithSynchronization(mcmRepository.Object, syncSpecification.Object);

            syncSpecification.Raise(s => s.OnSynchronizationTrigger += null, new EventArgs());
            Assert.AreEqual(1, permissionManager.GetFolders(1).UserPermissions.Count);
            Assert.AreEqual(1, permissionManager.GetFolders(1).GroupPermissions.Count);
        }
    }
}
