﻿using System.Collections.Generic;
using System.Linq;
using CHAOS.MCM.Data.DTO;
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
			IEnumerable<ObjectType> objects = MCMModule.ObjectType_Get(AdminCallContext);

			Assert.Greater(objects.ToList().Count, 0);
		}

		//[Test]
		//public void Should_Update_ObjectType()
		//{
		//    ScalarResult result = MCMModule.ObjectType_Update( AdminCallContext,
		//                                                       AssetObjectType.ID,
		//                                                       "new name" );

		//    Assert.AreEqual(1, result.Value);

		//    using( MCMDataContext db = MCMModule.DefaultMCMDataContext )
		//    {
		//        Assert.AreEqual( "new name", db.ObjectType_Get( AssetObjectType.ID, null ).First().Value );
		//    }
		//}

		//[Test, ExpectedException(typeof(InsufficientPermissionsException))]
		//public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Update_ObjectType()
		//{
		//    ScalarResult result = MCMModule.ObjectType_Update( AnonCallContext,
		//                                                       AssetObjectType.ID,
		//                                                       "new name" );

		//    Assert.AreEqual(-100, result.Value);
		//}

		//[Test]
		//public void Should_Delete_ObjectType()
		//{
		//    ScalarResult result = MCMModule.ObjectType_Delete( AdminCallContext,
		//                                                       DemoObjectType.ID );

		//    Assert.AreEqual(1, result.Value);
		//}

		//[Test, ExpectedException(typeof(InsufficientPermissionsException))]
		//public void Should_Throw_InsufficientPermisssionsException_If_User_Dont_Have_Permission_To_Delete_ObjectType()
		//{
		//    ScalarResult result = MCMModule.ObjectType_Delete( AnonCallContext,
		//                                                       AssetObjectType.ID);

		//    Assert.AreEqual(-100, result.Value);
		//}
    }
}
