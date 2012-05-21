using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CHAOS.MCM.Data.DTO;
using CHAOS.MCM.Module.Rights;
using NUnit.Framework;

namespace CHAOS.MCM.Test
{
    [TestFixture]
    public class FolderTest : MCMTestBase
    {
        [Test]
        public void Should_Get_Permissions_For_Folder()
        {
            var result     = FolderModule.GetPermission( AdminCallContext, SubFolder.ID );
            var permission = (FolderPermissions) result.AccumulatedPermission;

            Assert.AreEqual( uint.MaxValue, result.AccumulatedPermission );
           
            Assert.IsTrue( permission.HasFlag( FolderPermissions.Read ) );
            Assert.IsTrue( permission.HasFlag( FolderPermissions.Write ) );
            Assert.IsTrue( permission.HasFlag( FolderPermissions.CreateLink ) );
            Assert.IsTrue( permission.HasFlag( FolderPermissions.CreateUpdateObjects ) );
        }

        [Test]
        public void Should_Set_Permissions_On_Folder()
        {
            var result = FolderModule.SetPermission( AdminCallContext, UserAnonymous.GUID, null, EmptyFolder.ID, (uint) FolderPermissions.Read );

            Assert.AreEqual( 1, result.Value );
        }

		[Test]
		public void Should_Get_TopFolders()
		{
			IEnumerable<FolderInfo> folders = FolderModule.Get( AdminCallContext, null, null, null );

			Assert.Greater(folders.Count(), 0);
		}

		[Test]
		public void Should_Get_SubFolders()
		{
		    Thread.Sleep( 1000 );
		    IEnumerable<FolderInfo> folders = FolderModule.Get( AdminCallContext, null, null, TopFolder.ID);

		    foreach( FolderInfo folderInfo in folders )
		    {
		        Assert.AreEqual( TopFolder.ID, folderInfo.ParentID );
		    }

		    Assert.Greater(folders.Count(), 0);
		}

		//[Test]
		//public void Should_Delete_Folder()
		//{
		//    ScalarResult result = MCMModule.Folder_Delete( AdminCallContext, EmptyFolder.ID );

		//    Assert.Greater( (int) result.Value, 0 );
		//}

		//[Test, ExpectedException(typeof(FolderNotEmptyException))]
		//public void Should_Throw_FolderNotEmptyException_On_Delete_Folder()
		//{
		//    MCMModule.Folder_Delete( AdminCallContext, TopFolder.ID);
		//}

		//[Test, ExpectedException(typeof(InsufficientPermissionsException))]
		//public void Should_Throw_InsufficientPermissionsException_On_Delete_Folder()
		//{
		//    MCMModule.Folder_Delete( AnonCallContext, EmptyFolder.ID);
		//}

		//[Test]
		//public void Should_Update_Folder()
		//{
		//    ScalarResult result = MCMModule.Folder_Update( AdminCallContext, EmptyFolder.ID, "hellooo", null );

		//    Assert.Greater( (int) result.Value, 0 );
		//}

		//[Test, ExpectedException(typeof(InsufficientPermissionsException))]
		//public void Should_Throw_InsufficientPermissionsException_On_Update_Folder()
		//{
		//    MCMModule.Folder_Update( AnonCallContext, EmptyFolder.ID, "new", null );
		//}

		//[Test]
		//public void Should_Move_Folder()
		//{
		//    Assert.Ignore();
		//    ScalarResult result = MCMModule.Folder_Update( AdminCallContext, EmptyFolder.ID, null, null);

		//    Assert.Greater((int)result.Value, 0);
		//}

		[Test]
		public void Should_Create_Folder()
		{
		    FolderInfo folder = FolderModule.Create( AdminCallContext, null, "hellooo", TopFolder.ID, FolderType.ID);

		    Assert.AreEqual( "hellooo", folder.Name );
		    Assert.AreEqual( TopFolder.ID, folder.ParentID );
		    Assert.AreEqual( FolderType.ID, folder.FolderTypeID );
		}

        //[Test]
        //public void Should_Create_TopFolder()
        //{
        //    FolderInfo folder = FolderModule.Create( AdminCallContext, SubscriptionInfo.GUID, "hellooo", null, FolderType.ID);

        //    Assert.AreEqual( "hellooo", folder.Name);
        //    Assert.IsNull( folder.ParentID);
        //    Assert.AreEqual( FolderType.ID, folder.FolderTypeID);
        //}

		//[Test, ExpectedException(typeof(InsufficientPermissionsException))]
		//public void Should_Throw_InsufficientPermissionsException_On_Create_Folder()
		//{
		//    MCMModule.Folder_Create( AnonCallContext,null, "not allowed", TopFolder.ID, FolderType.ID);
		//}
    }
}
