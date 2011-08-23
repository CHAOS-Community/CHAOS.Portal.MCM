using System.Linq;
using Geckon.MCM.Data.Linq;
using Geckon.Portal.Core.Exception;
using Geckon.Portal.Core.Standard.Extension;
using Geckon.Portal.Data.Dto;
using Geckon.Portal.Extensions.Standard.Test;
using NUnit.Framework;

namespace Geckon.MCM.Module.Standard.Test
{
    [TestFixture]
    public class FolderTypeTest : BaseTest
    {
        [Test]
        public void Should_Get_FolderType()
        {
            Assert.Greater( MCMModule.FolderType_Get( new CallContext( new MockCache(), new MockSolr(), Session.SessionID.ToString() ), null, null ).Count(),0 );
        }

        [Test]
        public void Should_Create_FolderType()
        {
            FolderType folderType = MCMModule.FolderType_Create(new CallContext(new MockCache(), new MockSolr(), AdminSession.SessionID.ToString()), "name");

            Assert.AreEqual("name", folderType.Name);
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
        public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Create_FolderType()
        {
            MCMModule.FolderType_Create(new CallContext(new MockCache(), new MockSolr(), Session.SessionID.ToString()), "name");
        }

        [Test]
        public void Should_Delete_FolderType()
        {
            ScalarResult result = MCMModule.FolderType_Delete(new CallContext(new MockCache(), new MockSolr(), AdminSession.SessionID.ToString()), FolderType.ID);

            Assert.AreEqual(1, result.Value);
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
        public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Delete_FolderType()
        {
            Assert.AreNotEqual(-100, MCMModule.FolderType_Delete(new CallContext(new MockCache(), new MockSolr(), Session.SessionID.ToString()), FolderType.ID));
        }

        [Test]
        public void Should_Update_FolderType()
       { 
            ScalarResult result = MCMModule.FolderType_Update(new CallContext(new MockCache(), new MockSolr(), AdminSession.SessionID.ToString()), FolderType.ID, "name");

            Assert.AreEqual(1, result.Value);
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
        public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Update_FolderType()
        {
            Assert.AreNotEqual(-100, MCMModule.FolderType_Update(new CallContext(new MockCache(), new MockSolr(), Session.SessionID.ToString()), FolderType.ID, "name"));
        }
    }
}
