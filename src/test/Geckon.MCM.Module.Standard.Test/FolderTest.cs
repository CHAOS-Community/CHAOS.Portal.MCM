using System.Collections.Generic;
using System.Linq;
using Geckon.MCM.Data.Linq;
using Geckon.Portal.Core.Standard.Extension;
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
            IEnumerable<Folder> folders = MCMModule.Folder_Get( new CallContext( new MockCache(),new MockSolr(), AdminSession.SessionID.ToString()),null, null );

            Assert.Greater( folders.Count(), 0 );
        }
    }
}
