using System.Linq;
using NUnit.Framework;

namespace CHAOS.MCM.Test
{
    [TestFixture]
    public class FolderTypeTest : MCMTestBase
    {
		[Test]
		public void Should_Get_FolderType()
		{
			Assert.Greater( MCMModule.FolderType_Get(AnonCallContext, null, null).Count(), 0 );
		}

	   // [Test]
	   // public void Should_Create_FolderType()
	   // {
	   //     FolderType folderType = MCMModule.FolderType_Create( AdminCallContext, "name");

	   //     Assert.AreEqual("name", folderType.Name);
	   // }

	   // [Test, ExpectedException(typeof(InsufficientPermissionsException))]
	   // public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Create_FolderType()
	   // {
	   //     MCMModule.FolderType_Create( AnonCallContext, "name");
	   // }

	   // [Test]
	   // public void Should_Delete_FolderType()
	   // {
	   //     ScalarResult result = MCMModule.FolderType_Delete( AdminCallContext, FolderTestType.ID);

	   //     Assert.AreEqual(1, result.Value);
	   // }

	   // [Test, ExpectedException(typeof(InsufficientPermissionsException))]
	   // public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Delete_FolderType()
	   // {
	   //     Assert.AreNotEqual(-100, MCMModule.FolderType_Delete( AnonCallContext, FolderType.ID));
	   // }

	   // [Test]
	   // public void Should_Update_FolderType()
	   //{ 
	   //     ScalarResult result = MCMModule.FolderType_Update( AdminCallContext, FolderType.ID, "name");

	   //     Assert.AreEqual(1, result.Value);
	   // }

	   // [Test, ExpectedException(typeof(InsufficientPermissionsException))]
	   // public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Update_FolderType()
	   // {
	   //     Assert.AreNotEqual(-100, MCMModule.FolderType_Update( AnonCallContext, FolderType.ID, "name"));
	   // }
    }
}
