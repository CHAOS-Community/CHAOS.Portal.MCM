﻿using System;
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
		IList<Guid> GroupGuids = new List<Guid>();
        PermissionManager topFolder;
		//IList<int> directFolderIDs;

        public FolderRightsTest()
        {
			//directFolderIDs = new List<int>();
            topFolder = new PermissionManager();

			GroupGuids.Add( new Guid( "421a5b56-67e1-4a02-ab12-f04cb9d2d90c" ) );

            foreach( Folder folder in LoadFolders( new DirectoryInfo( "C:\\" ) ).GetSubFolders() )
            {
                topFolder.AddFolder( folder );
            }

			Random rand = new Random(1337);

        	for( int i = 1; i < 30000; i++ )
        	{
				int id = rand.Next( 1, 30000 );
        		topFolder.GetFolder( id ).AddUser( Guid.NewGuid(), FolderPermissions.Read );
        	}

			for( int i = 0; i < 5000; i++ )
        	{
        		int id = rand.Next(1, 30000);
				topFolder.GetFolder( id ).AddGroup( Guid.NewGuid(), FolderPermissions.Read );
        	}

			//for (int i = 1; i < 35000; i++)
			//{
			//    topFolder.GetFolder(i).AddUser(UserGuid, FolderPermissions.Read);
			//    directFolderIDs.AddFolder(i);
			//}

			topFolder.AddUser( 1, UserGuid, FolderPermissions.Read);
			topFolder.AddUser( 3, UserGuid, FolderPermissions.Read);
			topFolder.AddGroup( 5, GroupGuids.First(), FolderPermissions.Read);
			topFolder.AddGroup( 55, GroupGuids.First(), FolderPermissions.Read);
			topFolder.AddUser( 1784, UserGuid, FolderPermissions.Read);

			//directFolderIDs.Add(1);
			//directFolderIDs.Add(3);
			//directFolderIDs.Add(5);
			//directFolderIDs.Add(1784);
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
		
        [Test, ExpectedException( typeof( KeyNotFoundException ) )]
        public void Should_Throw_KeyNotFoundException_If_ID_Is_Not_Found()
        {
            topFolder.GetFolder( 100000 );
        }

		[Test]
		public void Should_Find_Users_TopFolders()
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();

			foreach( Folder folder in topFolder.GetFolders( UserGuid, GroupGuids ) )
			{
				if (!new[] { 1, 5, 1784, 55 }.Contains(folder.ID))
					Assert.Fail();
			}

			Console.WriteLine( "{0}ms",sw.ElapsedMilliseconds );
		}

		[Test]
		public void Should_Get_SubFolders()
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();

			Assert.AreEqual( 24, topFolder.GetFolders( UserGuid, GroupGuids, 5 ).Count() );

			Console.WriteLine("{0}ms", sw.ElapsedMilliseconds);
		}

		[Test]
		public void Should_Not_Return_Folders_Without_Permission()
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();

			Assert.AreEqual( 0, topFolder.GetFolders( UserGuid, GroupGuids, 48 ).Count() );

			Console.WriteLine("{0}ms", sw.ElapsedMilliseconds);
		}

		[Test]
		public void Should_Check_If_User_Has_Permissions()
		{
			Assert.IsTrue( topFolder.GetFolder( 5 ).DoesUserOrGroupHavePersmission( UserGuid, GroupGuids, FolderPermissions.Read ) );
		}

		[Test]
		public void Should_Check_If_User_Has_Inherited_Permissions()
		{
			Assert.IsTrue( topFolder.GetFolder( 4 ).DoesUserOrGroupHavePersmission( UserGuid, GroupGuids, FolderPermissions.Read ) );
		}

		[Test]
		public void Should_Return_False_If_User_Doesnt_Have_Permission()
		{
			Assert.IsFalse( topFolder.GetFolder( 48 ).DoesUserOrGroupHavePersmission( UserGuid, GroupGuids, FolderPermissions.Read ) );
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
