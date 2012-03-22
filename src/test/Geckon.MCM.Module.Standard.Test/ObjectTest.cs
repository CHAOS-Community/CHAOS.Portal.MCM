using CHAOS.MCM.Data.DTO;
using CHAOS.Portal.Data.DTO;
using Geckon.Portal.Core.Exception;
using NUnit.Framework;

namespace Geckon.MCM.Module.Standard.Test
{
    [TestFixture]
    public class ObjectTest : BaseTest
    {
        //[Test]
        //public void Should_Get_Object()
        //{
        //    IList<string> guids = new List<string>();
        //    guids.AddFolder( Object1.GUID.ToString() );

        //    IEnumerable<Object1> objectz = MCMModule.Object_Get( AdminCallContext, null, true, false, null, TopFolder.ID );

        //    Assert.Greater( objectz.Count(), 0 );
        //}

		//[Test]
		//public void Should_Create_Object()
		//{
		//    System.Guid guid = System.Guid.NewGuid();

		//    Object objectz = MCMModule.Object_Create( AdminCallContext, guid, AssetObjectType.ID, TopFolder.ID );

		//    Assert.AreEqual( guid.ToString(), objectz.GUID.ToString() );
		//}
        
		//[Test]
		//public void Should_Delete_Object()
		//{
		//    ScalarResult result = MCMModule.Object_Delete( AdminCallContext, Object1.GUID, TopFolder.ID );

		//    Assert.AreEqual( 1, result.Value );
		//}

		//[Test]
		//public void Should_Put_Object_In_Folder()
		//{
		//    ScalarResult result = MCMModule.Object_PutInFolder( AdminCallContext, Object1.GUID, EmptyFolder.ID, ObjectFolderLink.ID );

		//    Assert.AreEqual(1, result.Value);
		//}

		//[Test, ExpectedException(typeof(InsufficientPermissionsExcention))]
		//public void Should_Not_Put_Object_In_Folder_With_Insufficient_Permissions()
		//{
		//    MCMModule.Object_PutInFolder(AnonCallContext, Object1.GUID, EmptyFolder.ID, ObjectFolderLink.ID);
		//}

        //[Test]
        //public void Should_Get_Index_Fields_From_Object()
        //{
        //    Object obje = MCMModule.Object_Get(AdminCallContext, null, true, false, null, TopFolder.ID).First();

        //    Assert.AreEqual("title\r\nabstract\r\ndescription\r\n", obje.GetIndexableFields().Where(field => field.Key == "1_en_all").First().Value);
        //}
    }
}
