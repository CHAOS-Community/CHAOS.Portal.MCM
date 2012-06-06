using System.Collections.Generic;
using System.Linq;
using CHAOS.MCM.Data.DTO;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Portal.Exception;
using NUnit.Framework;

namespace CHAOS.MCM.Test
{
    [TestFixture]
    public class ObjectTypeTest : MCMTestBase
    {
		[Test]
		public void Should_Create_ObjectType()
		{
		    var objectType = ObjectTypeModule.Create( AdminCallContext, "MyObjectType" );

		    Assert.AreEqual( "MyObjectType", objectType.Name );
		}

		[Test, ExpectedException( typeof( InsufficientPermissionsException ) )]
		public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Create_ObjectType()
		{
		    ObjectTypeModule.Create( AnonCallContext, "MyObjectType" );
		}

		[Test]
		public void Should_Get_All_ObjectTypes()
		{
            IEnumerable<ObjectType> objects = ObjectTypeModule.Get(AdminCallContext);

			Assert.Greater(objects.ToList().Count, 0);
		}

        [Test]
        public void Should_Update_ObjectType()
        {
            var result = ObjectTypeModule.Update( AdminCallContext, AssetObjectType.ID, "new name" );

            Assert.AreEqual( 1, result.Value );

            using( var db = MCMModule.DefaultMCMEntities )
            {
                Assert.AreEqual("new name", db.ObjectType_Get( (int?) AssetObjectType.ID, null ).First().Name );
            }
        }

		[Test, ExpectedException(typeof(InsufficientPermissionsException))]
		public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Update_ObjectType()
		{
            ScalarResult result = ObjectTypeModule.Update( AnonCallContext,
                                                           DemoObjectType.ID,
		                                                   "new name" );

		    Assert.AreEqual(-100, result.Value);
		}

		[Test]
		public void Should_Delete_ObjectType()
		{
            ScalarResult result = ObjectTypeModule.Delete( AdminCallContext,
		                                                   AssetObjectType.ID );

		    Assert.AreEqual(1, result.Value);
		}

		[Test, ExpectedException(typeof(InsufficientPermissionsException))]
		public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Delete_ObjectType()
		{
            ScalarResult result = ObjectTypeModule.Delete( AnonCallContext,
		                                                   DemoObjectType.ID);

		    Assert.AreEqual(-100, result.Value);
		}
    }
}
