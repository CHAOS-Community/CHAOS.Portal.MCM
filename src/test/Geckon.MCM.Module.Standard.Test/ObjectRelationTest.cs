using System.Collections.Generic;
using System.Linq;
using CHAOS.MCM.Data.DTO;
using Geckon.MCM.Core.Exception;
using NUnit.Framework;

namespace Geckon.MCM.Module.Standard.Test
{
    [TestFixture]
    public class ObjectRelationTest : BaseTest
    {
		//[Test]
		//public void Should_Create_ObjectRelation()
		//{
		//    int result = MCMModule.ObjectRelation_Create( AdminCallContext, Object1.GUID, Object2.GUID, ObjectContains.ID, null ).Value;

		//    Assert.AreEqual( 1, result );
		//}

		//[Test, ExpectedException( typeof( ObjectRelationAlreadyExistException ) )]
		//public void Should_Throw_Exception_If_Same_Relation_Is_Created_Twice()
		//{
		//    MCMModule.ObjectRelation_Create( AdminCallContext, Object1.GUID, Object2.GUID, ObjectContains.ID, null );
		//    MCMModule.ObjectRelation_Create( AdminCallContext, Object1.GUID, Object2.GUID, ObjectContains.ID, null );
		//}

		//[Test]
		//public void Should_Get_ObjectRelations()
		//{
		//    MCMModule.ObjectRelation_Create(AdminCallContext, Object1.GUID, Object2.GUID, ObjectContains.ID, null);

		//    using( MCMDataContext db = MCMModule.DefaultMCMDataContext )
		//    {
		//        IList<Object> objects = db.Object_Get( true, false, false, true, true, Object1.ID, null, null, 0, 10).ToList();

		//        Assert.AreEqual( Object2.GUID, objects.First().ObjectRealtions[0].Object2GUID );
		//        Assert.AreEqual( Object2.GUID, objects.First().RelatedObjects[0].GUID );
		//        Assert.Greater( objects.Select( obj => obj.GetIndexableFields().Where( field => field.Key == "rm1_en_all").Select( field => field ) ).Count(), 0 );
		//    }
		//}

		//[Test]
		//public void Should_Delete_ObjectRelation()
		//{
		//    MCMModule.ObjectRelation_Create( AdminCallContext, Object1.GUID, Object2.GUID, ObjectContains.ID, null );

		//    int result = MCMModule.ObjectRelation_Delete( AdminCallContext, Object1.GUID, Object2.GUID, ObjectContains.ID ).Value;

		//    Assert.AreEqual( 1, result );
		//}
    }
}
