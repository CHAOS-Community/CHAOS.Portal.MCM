using System.Linq;
using CHAOS.MCM.Data.EF;
using NUnit.Framework;
using Object = CHAOS.MCM.Data.DTO.Object;

namespace Geckon.MCM.Module.Standard.Test
{
    [TestFixture]
    public class ObjectTest : BaseTest
    {
		[Test]
		public void Should_Create_Object()
		{
			UUID guid = new UUID();

			Object objectz = MCMModule.Object_Create( AdminCallContext, guid, AssetObjectType.ID, TopFolder.ID );

			Assert.AreEqual( guid.ToString(), objectz.GUID.ToString() );
		}

        [Test]
        public void Should_Get_Objects_ByFolderID()
        {
            using( MCMEntities db = new MCMEntities() )
            {
                var list = db.Object_Get( null, true, true, false, true, 0, 10 ).ToList();

                Assert.AreEqual( 1, list[0].Folders.First().FolderID );
                Assert.AreEqual( 3, list[1].Folders.First().FolderID );
                Assert.AreEqual( 1, list[2].Folders.First().FolderID );
            }
        }
         
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

		//[Test, ExpectedException(typeof(InsufficientPermissionsException))]
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
