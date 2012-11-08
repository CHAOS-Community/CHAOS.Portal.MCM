using System;
using System.Linq;
using CHAOS.MCM.Permission;
using CHAOS.MCM.Permission.InMemory;
using Moq;
using NUnit.Framework;

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
            var group   = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);

            PermissionManager.AddFolder(folder1);
            PermissionManager.AddFolder(folder2);
            PermissionManager.GetFolders(folder1.ID).AddGroup(group.Object);

            Assert.AreEqual(group.Object.Guid, folder2.GroupPermissions.First().Key);
            Assert.AreEqual(group.Object.Permission, folder2.GroupPermissions.First().Value);
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
    }
}
