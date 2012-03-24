using CHAOS.MCM.Data.DTO;
using CHAOS.Portal.Data.DTO;
using Geckon.Common.Extensions;
using Geckon.Portal.Core.Exception;
using NUnit.Framework;

namespace Geckon.MCM.Module.Standard.Test
{
    [TestFixture]
    public class LanguageTest : BaseTest
    {
		[Test]
		public void Should_Get_Language()
		{
			Assert.Greater( MCMModule.Language_Get(AnonCallContext, null, null).Count(), 0 );
		}

		//[Test]
		//public void Should_Create_Language()
		//{
		//    Language lan = MCMModule.Language_Create( AdminCallContext, "name", "code" );

		//    Assert.AreEqual("name", lan.Name);
		//    Assert.AreEqual("code", lan.LanguageCode);
		//}

		//[Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
		//public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Create_Language()
		//{
		//    Assert.AreNotEqual(-100, MCMModule.Language_Create( AnonCallContext, "name", "code" ) );
		//}

		//[Test]
		//public void Should_Delete_Language()
		//{
		//    ScalarResult result = MCMModule.Language_Delete( AdminCallContext, Afrikaans.LanguageCode );

		//    Assert.AreEqual(1, result.Value);
		//}

		//[Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
		//public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Delete_Language()
		//{
		//    Assert.AreNotEqual(-100, MCMModule.Language_Delete( AnonCallContext,Afrikaans.LanguageCode ) );
		//}

		//[Test]
		//public void Should_Update_Language()
		//{
		//    ScalarResult result = MCMModule.Language_Update( AdminCallContext, Afrikaans.LanguageCode, "name" );

		//    Assert.AreEqual(1, result.Value);
		//}

		//[Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
		//public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Update_Language()
		//{
		//    Assert.AreNotEqual(-100, MCMModule.Language_Update( AnonCallContext, Afrikaans.LanguageCode, "name"));
		//}
    }
}
