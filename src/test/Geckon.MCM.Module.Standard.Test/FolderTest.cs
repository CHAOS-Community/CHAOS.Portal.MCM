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
    }
}
