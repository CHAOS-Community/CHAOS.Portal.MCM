using System;
using System.Collections.Generic;
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

            Assert.AreEqual("folder 1", PermissionManager.GetFolder(1).Name);
            Assert.AreEqual("folder 2", PermissionManager.GetFolder(2).Name);
            Assert.AreEqual(1, PermissionManager.GetFolder(1).ID);
            Assert.AreEqual(2, PermissionManager.GetFolder(2).ID);
        }

        [Test]
        public void Should_Add_User_to_folder_with_permission()
        {
            var folder1 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)1)
                                             .SetupProperty(f => f.GroupPermissions, new Dictionary<Guid, FolderPermission>())
                                             .SetupProperty(f => f.UserPermissions, (new Dictionary<Guid, FolderPermission>()));
            var user    = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);
            
            PermissionManager.AddFolder(folder1.Object);
            PermissionManager.AddUser( folder1.Object.ID, user.Object );
            folder1.VerifyGet(f => f.UserPermissions, Times.AtLeastOnce());
        }

        [Test]
        public void Should_assocociate_inherit_userpermissions_when_adding_subfolder()
        {
            var folder1 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)1)
                                             .SetupProperty(f => f.Name, "folder 1")
                                             .SetupProperty(f => f.GroupPermissions, new Dictionary<Guid, FolderPermission>())
                                             .SetupProperty(f => f.UserPermissions, new Dictionary<Guid, FolderPermission>());
            var folder2 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)2)
                                             .SetupProperty(f => f.Name, "folder 2")
                                             .SetupProperty(f => f.GroupPermissions, new Dictionary<Guid, FolderPermission>())
                                             .SetupProperty(f => f.UserPermissions, new Dictionary<Guid, FolderPermission>())
                                             .SetupProperty(f => f.ParentFolder, folder1.Object);
            var user    = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);

            PermissionManager.AddFolder(folder1.Object);
            PermissionManager.AddUser(folder1.Object.ID, user.Object);
            PermissionManager.AddFolder(folder2.Object);

            var result = PermissionManager.GetFolder(folder2.Object.ID);

            Assert.AreEqual("folder 2", result.Name);
            Assert.AreEqual(1, result.UserPermissions.Count);
            Assert.AreEqual("39f26c89-5e6c-46d5-af3a-bc14a7e1486b", result.UserPermissions.First().Key.ToString());
            
            folder1.VerifyGet(f => f.UserPermissions);
        }

        [Test]
        public void Should_assocociate_subfolders_with_parent_on_addfolder()
        {
            var folder1 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)1)
                                             .SetupProperty(f => f.Name, "folder 1")
                                             .SetupProperty(f => f.GroupPermissions, new Dictionary<Guid, FolderPermission>())
                                             .SetupProperty(f => f.UserPermissions, new Dictionary<Guid, FolderPermission>());
            var folder2 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)2)
                                             .SetupProperty(f => f.Name, "folder 2")
                                             .SetupProperty(f => f.GroupPermissions, new Dictionary<Guid, FolderPermission>())
                                             .SetupProperty(f => f.UserPermissions, new Dictionary<Guid, FolderPermission>())
                                             .SetupProperty(f => f.ParentFolder, folder1.Object);

            PermissionManager.AddFolder(folder1.Object);
            PermissionManager.AddFolder(folder2.Object);

            folder1.Verify(f => f.AddSubFolder(folder2.Object));
        }

        [Test]
        public void Should_propagate_userpermissions_to_subfolders_when_adding_a_user()
        {
            var folder1 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)1)
                                             .SetupProperty(f => f.Name, "folder 1")
                                             .SetupProperty(f => f.GroupPermissions, new Dictionary<Guid, FolderPermission>())
                                             .SetupProperty(f => f.UserPermissions, new Dictionary<Guid, FolderPermission>());
            var folder2 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)2)
                                             .SetupProperty(f => f.Name, "folder 2")
                                             .SetupProperty(f => f.GroupPermissions, new Dictionary<Guid, FolderPermission>())
                                             .SetupProperty(f => f.UserPermissions, new Dictionary<Guid, FolderPermission>())
                                             .SetupProperty(f => f.ParentFolder, folder1.Object);
            var user    = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);

            folder1.Setup(f => f.GetSubFolders()).Returns( new[] {folder2.Object});

            PermissionManager.AddFolder(folder1.Object);
            PermissionManager.AddFolder(folder2.Object);
            PermissionManager.AddUser(folder1.Object.ID, user.Object);

            Assert.AreEqual(user.Object.Guid, folder2.Object.UserPermissions.First().Key);
            Assert.AreEqual(user.Object.Permission, folder2.Object.UserPermissions.First().Value);
        }

        [Test]
        public void Should_propagate_grouppermissions_to_subfolders_when_adding_a_group()
        {
            var folder1 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)1)
                                             .SetupProperty(f => f.Name, "folder 1")
                                             .SetupProperty(f => f.GroupPermissions, new Dictionary<Guid, FolderPermission>())
                                             .SetupProperty(f => f.UserPermissions, new Dictionary<Guid, FolderPermission>());
            var folder2 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)2)
                                             .SetupProperty(f => f.Name, "folder 2")
                                             .SetupProperty(f => f.GroupPermissions, new Dictionary<Guid, FolderPermission>())
                                             .SetupProperty(f => f.UserPermissions, new Dictionary<Guid, FolderPermission>())
                                             .SetupProperty(f => f.ParentFolder, folder1.Object);
            var group   = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);

            folder1.Setup(f => f.GetSubFolders()).Returns(new[] { folder2.Object });

            PermissionManager.AddFolder(folder1.Object);
            PermissionManager.AddFolder(folder2.Object);
            PermissionManager.AddGroup(folder1.Object.ID, group.Object);

            Assert.AreEqual(group.Object.Guid, folder2.Object.GroupPermissions.First().Key);
            Assert.AreEqual(group.Object.Permission, folder2.Object.GroupPermissions.First().Value);
        }

        [Test]
        public void Should_Get_TopFolders()
        {
            var folder1 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)1)
                                             .SetupProperty(f => f.GroupPermissions, new Dictionary<Guid, FolderPermission>())
                                             .SetupProperty(f => f.UserPermissions, new Dictionary<Guid, FolderPermission>());
            var folder2 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)2)
                                             .SetupProperty(f => f.GroupPermissions, new Dictionary<Guid, FolderPermission>())
                                             .SetupProperty(f => f.UserPermissions, new Dictionary<Guid, FolderPermission>())
                                             .SetupProperty(f => f.ParentFolder, folder1.Object);
            var folder3 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)3)
                                             .SetupProperty(f => f.ParentFolder, folder2.Object)
                                             .SetupProperty(f => f.GroupPermissions, new Dictionary<Guid, FolderPermission>())
                                             .SetupProperty(f => f.UserPermissions, new Dictionary<Guid, FolderPermission>());
            var folder4 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)4)
                                             .SetupProperty(f => f.GroupPermissions, new Dictionary<Guid, FolderPermission>())
                                             .SetupProperty(f => f.UserPermissions, new Dictionary<Guid, FolderPermission>());
            var perm1   = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);
            var perm2   = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Read);
            
            folder1.Setup(f => f.GetSubFolders()).Returns(new[] { folder2.Object });
            folder2.Setup(f => f.GetSubFolders()).Returns(new[] { folder3.Object });

            PermissionManager.AddFolder(folder1.Object);
            PermissionManager.AddFolder(folder2.Object);
            PermissionManager.AddFolder(folder3.Object);
            PermissionManager.AddFolder(folder4.Object);
            PermissionManager.AddUser(folder2.Object.ID, perm2.Object);
            PermissionManager.AddUser(folder3.Object.ID, perm1.Object);
            PermissionManager.AddUser(folder4.Object.ID, perm1.Object);

            var topFolders = PermissionManager.GetTopFolders(FolderPermission.CreateUpdateObjects, perm1.Object.Guid).ToList();

            Assert.AreEqual(2, topFolders.Count());
            Assert.AreEqual(3, topFolders[0].ID);
            Assert.AreEqual(4, topFolders[1].ID);
        }
    }
}
