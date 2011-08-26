using System.Linq;
using Geckon.MCM.Data.Linq;
using Geckon.Portal.Core.Exception;
using Geckon.Portal.Core.Standard.Extension;
using Geckon.Portal.Data;
using Geckon.Portal.Extensions.Standard.Test;
using NUnit.Framework;

namespace Geckon.MCM.Module.Standard.Test
{
    [TestFixture]
    public class ObjectRealtionsTypeTest : BaseTest
    {
        [Test]
        public void Should_Get_ObjectRelationType()
        {
            Assert.Greater( MCMModule.ObjectRelationType_Get( new CallContext( new MockCache(), new MockSolr(), Session.SessionID.ToString() ), null, null ).Count(),0 );
        }

        [Test]
        public void Should_Create_ObjectRelationType()
        {
            ObjectRelationType objectRelationType = MCMModule.ObjectRelationType_Create(new CallContext(new MockCache(), new MockSolr(), AdminSession.SessionID.ToString()), "name");

            Assert.AreEqual("name", objectRelationType.Value);
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
        public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Create_ObjectRelationType()
        {
            MCMModule.ObjectRelationType_Create(new CallContext(new MockCache(), new MockSolr(), Session.SessionID.ToString()), "name" );
        }

        [Test]
        public void Should_Delete_ObjectRelationType()
        {
            ScalarResult result = MCMModule.ObjectRelationType_Delete(new CallContext(new MockCache(), new MockSolr(), AdminSession.SessionID.ToString()), ObjectContains.ID );

            Assert.AreEqual(1, result.Value);
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
        public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Delete_ObjectRelationType()
        {
            Assert.AreNotEqual(-100, MCMModule.ObjectRelationType_Delete(new CallContext(new MockCache(), new MockSolr(), Session.SessionID.ToString()), ObjectContains.ID));
        }

        [Test]
        public void Should_Update_ObjectRelationType()
        {
            ScalarResult result = MCMModule.ObjectRelationType_Update(new CallContext(new MockCache(), new MockSolr(), AdminSession.SessionID.ToString()), ObjectContains.ID, "name");

            Assert.AreEqual(1, result.Value);
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
        public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Update_ObjectRelationType()
        {
            Assert.AreNotEqual(-100, MCMModule.ObjectRelationType_Update(new CallContext(new MockCache(), new MockSolr(), Session.SessionID.ToString()), ObjectContains.ID, "name"));
        }
    }
}
