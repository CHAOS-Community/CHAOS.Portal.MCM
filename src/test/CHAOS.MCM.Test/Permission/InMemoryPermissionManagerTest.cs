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
            var folder1 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint) 1)
                                             .SetupProperty(f => f.UserPermissions, (new Dictionary<Guid, IEntityPermission>()));
            var user    = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);
            
            PermissionManager.AddFolder(folder1.Object);
            PermissionManager.AddUser( folder1.Object.ID, user.Object );
            folder1.VerifyGet(f => f.UserPermissions, Times.AtLeastOnce());
        }

        [Test]
        public void Should_assocociate_parent_with_parent_folder()
        {
            var folder1 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)1)
                                             .SetupProperty(f => f.Name, "folder 1");
            var folder2 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)2)
                                             .SetupProperty(f => f.Name, "folder 2");
            
            PermissionManager.AddFolder(folder1.Object);
            PermissionManager.AddFolder(folder2.Object,folder1.Object.ID);

            var result = PermissionManager.GetFolder(folder2.Object.ID);

            Assert.AreEqual("folder 2", result.Name);
            folder2.VerifySet(f => f.Parent);
        }

        [Test]
        public void Should_assocociate_inherit_userpermissions_when_adding_subfolder()
        {
            var folder1 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)1)
                                             .SetupProperty(f => f.Name, "folder 1")
                                             .SetupProperty(f => f.UserPermissions, new Dictionary<Guid, IEntityPermission>());
            var folder2 = new Mock<IFolder>().SetupProperty(f => f.ID, (uint)2)
                                             .SetupProperty(f => f.Name, "folder 2")
                                             .SetupProperty(f => f.UserPermissions, new Dictionary<Guid, IEntityPermission>());
            var user    = new Mock<IEntityPermission>().SetupProperty(f => f.Guid, new Guid("39f26c89-5e6c-46d5-af3a-bc14a7e1486b"))
                                                       .SetupProperty(f => f.Permission, FolderPermission.Max);

            PermissionManager.AddFolder(folder1.Object);
            PermissionManager.AddUser(folder1.Object.ID, user.Object);
            PermissionManager.AddFolder(folder2.Object, folder1.Object.ID);

            var result = PermissionManager.GetFolder(folder2.Object.ID);

            Assert.AreEqual("folder 2", result.Name);
            Assert.AreEqual(1, result.UserPermissions.Count);
            Assert.AreEqual("39f26c89-5e6c-46d5-af3a-bc14a7e1486b", result.UserPermissions.Values.First().Guid);
        }
    }
}
