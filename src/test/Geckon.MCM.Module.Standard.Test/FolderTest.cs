using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Geckon.MCM.Core.Exception;
using Geckon.MCM.Data.Linq;
using Geckon.Portal.Core.Exception;
using Geckon.Portal.Data;
using NUnit.Framework;

namespace Geckon.MCM.Module.Standard.Test
{
    [TestFixture]
    public class FolderTest : BaseTest
    {
        [Test]
        public void Should_Get_TopFolders()
        {
            IEnumerable<FolderInfo> folders = MCMModule.Folder_Get( AdminCallContext,null, null,null );

            Assert.Greater( folders.Count(), 0 );
        }

        [Test]
        public void Should_Get_SubFolders()
        {
			Thread.Sleep( 1000 );
            IEnumerable<FolderInfo> folders = MCMModule.Folder_Get( AdminCallContext, null, null, TopFolder.ID);

            foreach( FolderInfo folderInfo in folders )
            {
                Assert.AreEqual( TopFolder.ID, folderInfo.ParentID );
            }

            Assert.Greater(folders.Count(), 0);
        }

        [Test]
        public void Should_Delete_Folder()
        {
            ScalarResult result = MCMModule.Folder_Delete( AdminCallContext, EmptyFolder.ID );

            Assert.Greater( (int) result.Value, 0 );
        }

        [Test, ExpectedException(typeof(FolderNotEmptyException))]
        public void Should_Throw_FolderNotEmptyException_On_Delete_Folder()
        {
            MCMModule.Folder_Delete( AdminCallContext, TopFolder.ID);
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
        public void Should_Throw_InsufficientPermissionsExcention_On_Delete_Folder()
        {
            MCMModule.Folder_Delete( AnonCallContext, EmptyFolder.ID);
        }

        [Test]
        public void Should_Update_Folder()
        {
            ScalarResult result = MCMModule.Folder_Update( AdminCallContext, EmptyFolder.ID, "hellooo", null );

            Assert.Greater( (int) result.Value, 0 );
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
        public void Should_Throw_InsufficientPermissionsExcention_On_Update_Folder()
        {
            MCMModule.Folder_Update( AnonCallContext, EmptyFolder.ID, "new", null );
        }

        [Test]
        public void Should_Move_Folder()
        {
			Assert.Ignore();
            ScalarResult result = MCMModule.Folder_Update( AdminCallContext, EmptyFolder.ID, null, null);

            Assert.Greater((int)result.Value, 0);
        }

        [Test]
        public void Should_Create_Folder()
        {
            FolderInfo folder = MCMModule.Folder_Create( AdminCallContext, SubscriptionInfo.GUID.ToString(), "hellooo", TopFolder.ID, FolderType.ID);

            Assert.AreEqual( "hellooo", folder.Title );
            Assert.AreEqual( TopFolder.ID, folder.ParentID );
            Assert.AreEqual( FolderType.ID, folder.FolderTypeID );
        }

        [Test]
        public void Should_Create_TopFolder()
        {
            FolderInfo folder = MCMModule.Folder_Create( AdminCallContext, SubscriptionInfo.GUID.ToString(), "hellooo", null, FolderType.ID);

            Assert.AreEqual("hellooo", folder.Title);
            Assert.IsNull(folder.ParentID);
            Assert.AreEqual(FolderType.ID, folder.FolderTypeID);
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
        public void Should_Throw_InsufficientPermissionsExcention_On_Create_Folder()
        {
            MCMModule.Folder_Create( AnonCallContext,null, "not allowed", TopFolder.ID, FolderType.ID);
        }
    }
}
