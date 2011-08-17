using Geckon.MCM.Data.Linq.DTO;
using Geckon.Portal.Core.Standard.Extension;
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
    }
}
