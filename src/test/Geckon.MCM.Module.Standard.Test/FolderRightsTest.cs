using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Geckon.MCM.Module.Standard.Rights;
using NUnit.Framework;
using System.IO;

namespace Geckon.MCM.Module.Standard.Test
{
    [TestFixture]
    public class FolderRightsTest
    {
		Guid UserGuid = new Guid( "321a5b56-67e1-4a02-ab12-f04cb9d2d90c" );
        PermissionManager topFolder;
		IList<int> directFolderIDs;

        public FolderRightsTest()
        {
			directFolderIDs = new List<int>();
            topFolder = new PermissionManager();

            foreach( Folder folder in LoadFolders( new DirectoryInfo( "C:\\" ) ).GetSubFolders() )
            {
                topFolder.Add( folder );
            }

			topFolder.GetFolder(1).AddUser(UserGuid, (int)FolderPermissions.Read);
			topFolder.GetFolder(3).AddUser(UserGuid, (int)FolderPermissions.Read);
			topFolder.GetFolder(1784).AddUser(UserGuid, (int)FolderPermissions.Read);
			directFolderIDs.Add(1);
			directFolderIDs.Add(3);
			directFolderIDs.Add(1784);
        }

        [Test]
        public void Should_Count_Folders()
        {
            int indexCount  = topFolder.Count();
            int actualCount = topFolder.GetFolders().Aggregate( 0, ( total, next ) => total + next.Count() ) + topFolder.GetFolders().Count();

            Assert.AreEqual( actualCount, indexCount );
        }

        [Test]
        public void Should_Find_Folder_By_ID()
        {
            Folder folder = topFolder.GetFolder( 10000 );

            Assert.IsNotNull( folder );
        }
		
        [Test, ExpectedException(typeof(KeyNotFoundException))]
        public void Should_Throw_KeyNotFoundException_If_ID_Is_Not_Found()
        {
            topFolder.GetFolder( 100000 );
        }

		[Test]
		public void Should_Find_Users_TopFolders()
		{
			foreach( Folder folder in topFolder.GetTopFolders( UserGuid, directFolderIDs ) )
			{
				if( folder.ID != 1 && folder.ID != 1784 )
					Assert.Fail();
			}
		}

        private int index = 0;

        private Folder LoadFolders( DirectoryInfo dir )
        {
            Folder folder = new Folder(index++);

            try
            {
                foreach( DirectoryInfo info in dir.GetDirectories() )
                {
                    folder.Add( LoadFolders( info ) );
                }
            }
            catch( UnauthorizedAccessException )
            {

            }

            return folder;
        }
    }
}
