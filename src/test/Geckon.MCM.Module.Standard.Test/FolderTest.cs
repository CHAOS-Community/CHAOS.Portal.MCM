using System.Collections.Generic;
using System.Linq;
using Geckon.MCM.Core.Exception;
using Geckon.MCM.Data.Linq;
using Geckon.Portal.Core.Exception;
using Geckon.Portal.Core.Standard.Extension;
using Geckon.Portal.Data;
using Geckon.Portal.Extensions.Standard.Test;
using NUnit.Framework;

namespace Geckon.MCM.Module.Standard.Test
{
    [TestFixture]
    public class FolderTest : BaseTest
    {
        [Test]
        public void Should_Get_TopFolders()
        {
            IEnumerable<FolderInfo> folders = MCMModule.Folder_Get( new CallContext( new MockCache(),new MockSolr(), AdminSession.SessionID.ToString()),null, null,null );

            Assert.Greater( folders.Count(), 0 );
        }

        [Test]
        public void Should_Get_SubFolders()
        {
            IEnumerable<FolderInfo> folders = MCMModule.Folder_Get(new CallContext(new MockCache(), new MockSolr(), AdminSession.SessionID.ToString()), null, null, TopFolder.ID);

            foreach( FolderInfo folderInfo in folders )
            {
                Assert.AreEqual( TopFolder.ID, folderInfo.ParentID );
            }

            Assert.Greater(folders.Count(), 0);
        }

        [Test]
        public void Should_Delete_Folder()
        {
            ScalarResult result = MCMModule.Folder_Delete( new CallContext(new MockCache(), new MockSolr(), AdminSession.SessionID.ToString()), EmptyFolder.ID );

            Assert.Greater( (int) result.Value, 0 );
        }

        [Test, ExpectedException(typeof(FolderNotEmptyException))]
        public void Should_Throw_FolderNotEmptyException_On_Delete_Folder()
        {
            MCMModule.Folder_Delete(new CallContext(new MockCache(), new MockSolr(), AdminSession.SessionID.ToString()), TopFolder.ID);
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
        public void Should_Throw_InsufficientPermissionsExcention_On_Delete_Folder()
        {
            MCMModule.Folder_Delete(new CallContext(new MockCache(), new MockSolr(), Session.SessionID.ToString()), EmptyFolder.ID);
        }

        [Test]
        public void Should_Update_Folder()
        {
            ScalarResult result = MCMModule.Folder_Update(new CallContext(new MockCache(), new MockSolr(), AdminSession.SessionID.ToString()), EmptyFolder.ID, "hellooo", null, null );

            Assert.Greater( (int) result.Value, 0 );
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
        public void Should_Throw_InsufficientPermissionsExcention_On_Update_Folder()
        {
            MCMModule.Folder_Update(new CallContext(new MockCache(), new MockSolr(), Session.SessionID.ToString()), EmptyFolder.ID, null, TopFolder.ID, null );
        }

        [Test]
        public void Should_Move_Folder()
        {
            ScalarResult result = MCMModule.Folder_Update(new CallContext(new MockCache(), new MockSolr(), AdminSession.SessionID.ToString()), EmptyFolder.ID, null, TopFolder.ID, null);

            Assert.Greater((int)result.Value, 0);
        }

        [Test]
        public void Should_Create_Folder()
        {
            FolderInfo folder = MCMModule.Folder_Create(new CallContext(new MockCache(), new MockSolr(), AdminSession.SessionID.ToString()), SubscriptionInfo.GUID.ToString(), "hellooo", TopFolder.ID, FolderType.ID);

            Assert.AreEqual( "hellooo", folder.Title );
            Assert.AreEqual( TopFolder.ID, folder.ParentID );
            Assert.AreEqual( FolderType.ID, folder.FolderTypeID );
        }

        [Test]
        public void Should_Create_TopFolder()
        {
            FolderInfo folder = MCMModule.Folder_Create(new CallContext(new MockCache(), new MockSolr(), AdminSession.SessionID.ToString()), SubscriptionInfo.GUID.ToString(), "hellooo", null, FolderType.ID);

            Assert.AreEqual("hellooo", folder.Title);
            Assert.IsNull(folder.ParentID);
            Assert.AreEqual(FolderType.ID, folder.FolderTypeID);
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
        public void Should_Throw_InsufficientPermissionsExcention_On_Create_Folder()
        {
            MCMModule.Folder_Create(new CallContext(new MockCache(), new MockSolr(), Session.SessionID.ToString()),null, "not allowed", TopFolder.ID, FolderType.ID);
        }
    }
}
