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
    public class FormatTypeTest : BaseTest
    {
        [Test]
        public void Should_Get_FormatTypeTest()
        {
            Assert.Greater( MCMModule.FormatType_Get( AnonCallContext, null, null ).Count(),0 );
        }

        [Test]
        public void Should_Create_FormatType()
        {
            FormatType FormatType = MCMModule.FormatType_Create( AdminCallContext, "name");

            Assert.AreEqual("name", FormatType.Value);
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
        public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Create_FormatType()
        {
            MCMModule.FormatType_Create( AnonCallContext, "name");
        }

        [Test]
        public void Should_Delete_FormatType()
        {
            FormatType testType = MCMModule.FormatType_Create( AdminCallContext, "unitTest" );
            ScalarResult result = MCMModule.FormatType_Delete( AdminCallContext, testType.ID);

            Assert.AreEqual(1, result.Value);
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
        public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Delete_FormatType()
        {
            Assert.AreNotEqual(-100, MCMModule.FormatType_Delete( AnonCallContext, FormatType.ID));
        }

        [Test]
        public void Should_Update_FormatType()
       { 
            ScalarResult result = MCMModule.FormatType_Update( AdminCallContext, FormatType.ID, "name");

            Assert.AreEqual(1, result.Value);
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
        public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Update_FormatType()
        {
            Assert.AreNotEqual(-100, MCMModule.FormatType_Update( AnonCallContext, FormatType.ID, "name"));
        }
    }
}
