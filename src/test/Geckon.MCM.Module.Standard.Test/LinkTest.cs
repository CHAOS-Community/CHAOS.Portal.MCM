using CHAOS.MCM.Data.EF;
using Geckon.Portal.Core.Exception;
using NUnit.Framework;

namespace Geckon.MCM.Module.Standard.Test
{
    [TestFixture]
    public class LinkTest :BaseTest
    {
        [Test]
        public void Should_Create_Link()
        {
            MCMModule.Link_Create( AdminCallContext, Object1.GUID, EmptyFolder.ID );

            using( MCMEntities db = new MCMEntities() )
            {
                foreach( Folder folder in db.Folder_Get(null, Object1.GUID.ToByteArray()) )
                {
                    if( folder.ID == EmptyFolder.ID )
                    {
                        Assert.IsTrue( true );
                        return;
                    }
                }

                Assert.Fail("Object wasnt linked");
            }
        }

        [Test]
        public void Should_Update_Link()
        {
            MCMModule.Link_Create( AdminCallContext, Object1.GUID, SubFolder.ID );
            MCMModule.Link_Update( AdminCallContext, Object1.GUID, SubFolder.ID, EmptyFolder.ID );

            using( MCMEntities db = new MCMEntities() )
            {
                foreach( Folder folder in db.Folder_Get( null, Object1.GUID.ToByteArray() ) )
                {
                    if( folder.ID == EmptyFolder.ID )
                    {
                        Assert.IsTrue( true );
                        return;
                    }
                }

                Assert.Fail("Object wasnt linked");
            }
        }

        [Test]
        public void Should_Delete_Link()
        {
            MCMModule.Link_Create( AdminCallContext, Object1.GUID, EmptyFolder.ID );
            MCMModule.Link_Delete( AdminCallContext, Object1.GUID, EmptyFolder.ID );

            using( MCMEntities db = new MCMEntities() )
            {
                foreach( Folder folder in db.Folder_Get( null, Object1.GUID.ToByteArray() ) )
                {
                    if( folder.ID == EmptyFolder.ID )
                    {
                        Assert.Fail("Object wasnt linked");
                    }
                }
            }
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsException))]
        public void Should_Throw_Exception_If_Not_Enough_Permissions_To_Create_Link()
        {
            MCMModule.Link_Create( AnonCallContext, Object1.GUID, EmptyFolder.ID );
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsException))]
        public void Should_Throw_Exception_If_Not_Enough_Permissions_To_Update_Link()
        {
            MCMModule.Link_Update( AnonCallContext, Object1.GUID, TopFolder.ID, EmptyFolder.ID );
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsException))]
        public void Should_Throw_Exception_If_Not_Enough_Permissions_To_Delete_Link()
        {
            MCMModule.Link_Delete( AnonCallContext, Object1.GUID, TopFolder.ID );
        }
    }
}
