using System;
using System.Linq;
using CHAOS.MCM.Data;
using CHAOS.MCM.Data.Dto;
using CHAOS.MCM.Data.Dto.Standard;
using CHAOS.MCM.Permission;
using CHAOS.MCM.Permission.InMemory;
using Moq;
using NUnit.Framework;
using Folder = CHAOS.MCM.Permission.InMemory.Folder;
using FolderPermission = CHAOS.MCM.Permission.FolderPermission;
using IFolder = CHAOS.MCM.Permission.IFolder;

namespace CHAOS.MCM.Test.Permission
{
    [TestFixture]
    public class InMemoryPermissionManagerTest
    {
        public IPermissionManager PermissionManager { get; set; }

        [SetUp]
        public void SetUp()
        {
            PermissionManager = new InMemoryPermissionManager();
        }

        [Test]
        public void Should_add_folder_to_manager()
        {
            var folder = new Mock<IFolder>();

            PermissionManager.AddFolder(folder.Object);
        }

        [Test]
        public void Should_get_folder_by_ids()
        {
            var folder1 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)1)
                                             .SetupProperty(f => f.Name, "folder 1");
            var folder2 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)2)
                                             .SetupProperty(f => f.Name, "folder 2");

            PermissionManager.AddFolder(folder1.Object);
            PermissionManager.AddFolder(folder2.Object);

            Assert.AreEqual("folder 1", PermissionManager.GetFolders(1).Name);
            Assert.AreEqual("folder 2", PermissionManager.GetFolders(2).Name);
            Assert.AreEqual(1, PermissionManager.GetFolders(1).ID);
            Assert.AreEqual(2, PermissionManager.GetFolders(2).ID);
        }

        [Test]
        public void Should_Add_User_to_folder_with_permission()
        {
            var folder1 = new Folder{ID = 1};
            var user    = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);
            
            PermissionManager.AddFolder(folder1);
            PermissionManager.GetFolders(folder1.ID).AddUser(user.Object);
            
            Assert.AreEqual(1, folder1.UserPermissions.Count);
        }
        
        [Test]
        public void Should_assocociate_inherit_userpermissions_when_adding_subfolder()
        {
            var folder1 = new Folder { ID = 1, Name = "folder 1" };
            var folder2 = new Folder { ID = 2, Name = "folder 2", ParentFolder = folder1};
            var user    = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);

            PermissionManager.AddFolder(folder1);
            PermissionManager.GetFolders(folder1.ID).AddUser(user.Object);
            PermissionManager.AddFolder(folder2);

            var result = PermissionManager.GetFolders(folder2.ID);

            Assert.AreEqual("folder 2", result.Name);
            Assert.AreEqual(1, result.UserPermissions.Count);
            Assert.AreEqual("39f26c89-5e6c-46d5-af3a-bc14a7e1486b", result.UserPermissions.First().Key.ToString());          
        }

        [Test]
        public void Should_assocociate_subfolders_with_parent_on_addfolder()
        {
            var folder1 = new Folder {ID = 1};
            var folder2 = new Folder {ID = 2, ParentFolder = folder1};

            PermissionManager.AddFolder(folder1);
            PermissionManager.AddFolder(folder2);

            Assert.AreEqual(1, folder1.GetSubFolders().Count());
        }

        [Test]
        public void Should_propagate_userpermissions_to_subfolders_when_adding_a_user()
        {
            var folder1 = new Folder { ID = 1 };
            var folder2 = new Folder { ID = 1, ParentFolder = folder1 };
            var user    = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);

            PermissionManager.AddFolder(folder1);
            PermissionManager.AddFolder(folder2);
            PermissionManager.GetFolders(folder1.ID).AddUser(user.Object);

            Assert.AreEqual(user.Object.Guid, folder2.UserPermissions.First().Key);
            Assert.AreEqual(user.Object.Permission, folder2.UserPermissions.First().Value);
        }

        [Test]
        public void Should_propagate_grouppermissions_to_subfolders_when_adding_a_group()
        {
            var folder1 = new Folder { ID = 1 };
            var folder2 = new Folder { ID = 2, ParentFolder = folder1 };
            var folder3 = new Folder { ID = 3, ParentFolder = folder2 };
            var group   = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);

            PermissionManager.AddFolder(folder1);
            PermissionManager.AddFolder(folder2);
            PermissionManager.AddFolder(folder3);
            PermissionManager.GetFolders(folder1.ID).AddGroup(group.Object);

            Assert.AreEqual(group.Object.Guid, folder3.GroupPermissions.First().Key);
            Assert.AreEqual(group.Object.Permission, folder3.GroupPermissions.First().Value);
        }

        [Test]
        public void Should_Get_TopFolders()
        {
            var folder1 = new Folder { ID = 1 };
            var folder2 = new Folder { ID = 2, ParentFolder = folder1 };
            var folder3 = new Folder { ID = 3, ParentFolder = folder2 };
            var folder4 = new Folder { ID = 4 };
            var perm1   = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);
            var perm2   = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("40f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Read);
            
            PermissionManager.AddFolder(folder1);
            PermissionManager.AddFolder(folder2);
            PermissionManager.AddFolder(folder3);
            PermissionManager.AddFolder(folder4);
            PermissionManager.GetFolders(folder2.ID).AddGroup(perm2.Object);
            PermissionManager.GetFolders(folder3.ID).AddUser(perm1.Object);
            PermissionManager.GetFolders(folder4.ID).AddUser(perm1.Object);

            var topFolders = PermissionManager.GetFolders(FolderPermission.CreateUpdateObjects, perm1.Object.Guid, new[] { perm2.Object.Guid }).ToList();

            Assert.AreEqual(2, topFolders.Count());
            Assert.AreEqual(3, topFolders[0].ID);
            Assert.AreEqual(4, topFolders[1].ID);
        }

        [Test]
        public void Should_Get_SubFolders_With_Permission_Through_Group()
        {
            var folder1 = new Folder { ID = 1 };
            var folder2 = new Folder { ID = 2, ParentFolder = folder1 };
            var folder3 = new Folder { ID = 3, ParentFolder = folder2 };
            var folder4 = new Folder { ID = 4 };
            var perm1   = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);
            var perm2   = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("40f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Read);

            PermissionManager.AddFolder(folder1);
            PermissionManager.AddFolder(folder2);
            PermissionManager.AddFolder(folder3);
            PermissionManager.AddFolder(folder4);
            PermissionManager.GetFolders(folder1.ID).AddGroup(perm2.Object);

            var hasPermissions = PermissionManager.GetFolders(folder1.ID).DoesUserOrGroupHavePermission(perm1.Object.Guid, new[] { perm2.Object.Guid }, FolderPermission.Read );
            var folders        = PermissionManager.GetFolders(folder1.ID).GetSubFolders().Where(item => item.DoesUserOrGroupHavePermission(perm1.Object.Guid, new[] { perm2.Object.Guid }, FolderPermission.Read));

            Assert.IsTrue(hasPermissions);
            Assert.AreEqual(2, folders.First().ID);
        }

        [Test]
        public void Should_synchronize_with_permission_repository()
        {
            var entity               = new Mock<IEntityPermission>();
            var permissionRepository = new Mock<IPermissionRepository>();
            var syncSpecification    = new Mock<ISynchronizationSpecification>();
            entity.SetupProperty(p => p.Guid, new Guid("c86a1cfc-10de-4a51-8d7b-6ba8f985e273"))
                  .SetupProperty(p => p.Permission, FolderPermission.Read);

            permissionRepository.Setup(repo => repo.GetFolder()).Returns(new[] { new Data.Dto.Standard.Folder { ID = 1 } });
            permissionRepository.Setup(repo => repo.GetFolderUserJoin()).Returns(new[] { new FolderUserJoin { FolderID = 1, Permission = 1 } });
            permissionRepository.Setup(repo => repo.GetFolderGroupJoin()).Returns(new[] { new FolderGroupJoin { FolderID = 1, Permission = 1 } });

            PermissionManager.WithSynchronization(permissionRepository.Object, syncSpecification.Object);

            syncSpecification.Raise(s => s.OnSynchronizationTrigger += null, new EventArgs());

            permissionRepository.Verify(repo => repo.GetFolder(), Times.AtLeastOnce());
            permissionRepository.Verify(repo => repo.GetFolderGroupJoin(), Times.AtLeastOnce());
            permissionRepository.Verify(repo => repo.GetFolderUserJoin(), Times.AtLeastOnce());

            Assert.AreEqual(1, PermissionManager.GetFolders(1).UserPermissions.Count);
            Assert.AreEqual(1, PermissionManager.GetFolders(1).GroupPermissions.Count);
        }
    }
}
