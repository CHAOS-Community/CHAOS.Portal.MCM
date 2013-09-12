namespace Chaos.Mcm.Test.Extension.v5
{
    using System;
    using System.Collections.Generic;

    using Chaos.Mcm.Permission;
    using Chaos.Mcm.Permission.InMemory;
    using Chaos.Portal.Core.Indexing;
    using Chaos.Portal.Core.Indexing.Solr.Request;
    using Chaos.Portal.Core.Indexing.View;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class ObjectTest : TestBase
    {
        [Test]
        public void Get_WithGuidQuery_ShouldQueryViewWithIdAndFolders()
        {
            var extension  = Make_ObjectV5Extension();
            var view       = new Mock<IView>();
            var folder     = new Folder{ID = 1};
            var user       = Make_User();
            var query      = new SolrQuery(){Query = "GUID:00000000-0000-0000-0000-000000000001"};
            ViewManager.Setup(m => m.GetView("Object")).Returns(view.Object);
            PortalRequest.SetupGet(p => p.User).Returns(user);
            PermissionManager.Setup(m => m.GetFolders(FolderPermission.Read, user.Guid, It.IsAny<IEnumerable<Guid>>())).Returns(new[] { folder });

            extension.Get(query, null, true, true, true, true);

            view.Verify(m => m.Query(It.Is<IQuery>(q => q.Query == "(Id:00000000-0000-0000-0000-000000000001)AND(FolderAncestors:1)")));
        }

        [Test]
        public void Get_WithObjectTypeQuery_ShouldQueryViewWithObjectTypeIdAndFolders()
        {
            var extension  = Make_ObjectV5Extension();
            var view       = new Mock<IView>();
            var folder     = new Folder{ID = 1};
            var user       = Make_User();
            var query      = new SolrQuery(){Query = "ObjectTypeID:1"};
            ViewManager.Setup(m => m.GetView("Object")).Returns(view.Object);
            PortalRequest.SetupGet(p => p.User).Returns(user);
            PermissionManager.Setup(m => m.GetFolders(FolderPermission.Read, user.Guid, It.IsAny<IEnumerable<Guid>>())).Returns(new[] { folder });

            extension.Get(query, null, true, true, true, true);

            view.Verify(m => m.Query(It.Is<IQuery>(q => q.Query == "(ObjectTypeId:1)AND(FolderAncestors:1)")));
        }
    }
}