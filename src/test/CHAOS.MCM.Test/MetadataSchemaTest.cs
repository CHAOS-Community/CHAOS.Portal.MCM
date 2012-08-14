using System.Linq;
using CHAOS.MCM.Module;
using CHAOS.Portal.Exception;
using NUnit.Framework;

namespace CHAOS.MCM.Test
{
    [TestFixture]
    public class MetadataSchemaTest : MCMTestBase
    {
		[Test]
		public void Should_Get_All_MetadataSchemas()
		{
			var results = MetadataSchemaModule.Get( AdminCallContext, null );

			Assert.Greater(results.Count(), 0);
		}

        [Test]
        public void Should_Create_MetadataSchema()
        {
            var result = MetadataSchemaModule.Create( AdminCallContext, new UUID(), "test", "<xml />"  );

            Assert.AreEqual( "test", result.Name );
            Assert.AreEqual( "<xml />", result.SchemaXML.ToString() );
        }

        [Test]
        public void Should_Update_MetadataSchema()
        {
            var result = MetadataSchemaModule.Update( AdminCallContext, MetadataSchema.GUID, "test", "<xml />"  );

            Assert.AreEqual( 1, result.Value );
        }

        [Test]
        public void Should_Delete_MetadataSchema()
        {
			Assert.Ignore();
           // var result = MetadataSchemaModule.Delete( AdminCallContext, MetadataSchema2.GUID );

           // Assert.AreEqual( 1, result.Value );
        }

        [Test, ExpectedException( typeof( InsufficientPermissionsException ))]
        public void Should_Throw_InsufficientPermissionsException_On_Delete_MetadataSchema()
        {
            MetadataSchemaModule.Delete( AnonCallContext, MetadataSchema2.GUID );
        }

        [Test, ExpectedException( typeof( InsufficientPermissionsException ))]
        public void Should_Throw_InsufficientPermissionsException_On_Update_MetadataSchema()
        {
            MetadataSchemaModule.Update( AnonCallContext, MetadataSchema2.GUID, "test", "<xmmml />" );
        }
    }
}
