using System.Collections.Generic;
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
    public class ObjectTypeTest : BaseTest
    {
        [Test]
        public void Should_Create_ObjectType()
        {
            ObjectType objectType = MCMModule.ObjectType_Create( new CallContext( new MockCache(), new MockSolr(), AdminSession.SessionID.ToString() ), "MyObjectType" );

            Assert.AreEqual( "MyObjectType", objectType.Value );
        }

        [Test, ExpectedException( typeof( InsufficientPermissionsExcention ) )]
        public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Create_ObjectType()
        {
            MCMModule.ObjectType_Create( new CallContext( new MockCache(), new MockSolr(), Session.SessionID.ToString() ), "MyObjectType" );
        }

        [Test]
        public void Should_Get_All_ObjectTypes()
        {
            IEnumerable<ObjectType> objects = MCMModule.ObjectType_Get( new CallContext( new MockCache(), new MockSolr() , AdminSession.SessionID.ToString() ) );

            Assert.Greater( objects.ToList().Count, 0 );
        }

        [Test]
        public void Should_Update_ObjectType()
        {
            ScalarResult result = MCMModule.ObjectType_Update( new CallContext( new MockCache(), new MockSolr() , AdminSession.SessionID.ToString() ),
                                                               AssetObjectType.ID,
                                                               "new name" );

            Assert.AreEqual(1, result.Value);

            using( MCMDataContext db = MCMModule.DefaultMCMDataContext )
            {
                Assert.AreEqual( "new name", db.ObjectType_Get( AssetObjectType.ID, null ).First().Value );
            }
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
        public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Update_ObjectType()
        {
            ScalarResult result = MCMModule.ObjectType_Update( new CallContext( new MockCache(), new MockSolr() , Session.SessionID.ToString() ),
                                                               AssetObjectType.ID,
                                                               "new name" );

            Assert.AreEqual(-100, result.Value);
        }

        [Test]
        public void Should_Delete_ObjectType()
        {
            ScalarResult result = MCMModule.ObjectType_Delete(new CallContext(new MockCache(), new MockSolr(), AdminSession.SessionID.ToString()),
                                                               DemoObjectType.ID );

            Assert.AreEqual(1, result.Value);
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
        public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Delete_ObjectType()
        {
            ScalarResult result = MCMModule.ObjectType_Delete(new CallContext(new MockCache(), new MockSolr(), Session.SessionID.ToString()),
                                                               AssetObjectType.ID);

            Assert.AreEqual(-100, result.Value);
        }
    }
}
