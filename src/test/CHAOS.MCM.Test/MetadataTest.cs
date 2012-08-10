using CHAOS.MCM.Core.Exception;
using NUnit.Framework;

namespace CHAOS.MCM.Test
{
    [TestFixture]
    public class MetadataTest : MCMTestBase
    {
		[Test]
		public void Should_Set_Metadata()
		{
			var result = MCMModule.Metadata_Set( AdminCallContext, Object1.GUID, MetadataSchema.GUID, Afrikaans.LanguageCode, 1, "<demo><title>test</title><abstract>test</abstract><description>test</description></demo>" );

			Assert.AreEqual(1, result.Value);
		}

        [Test]
		public void Should_Set_Metadata_On_Other_Object()
		{
			var result = MCMModule.Metadata_Set( AdminCallContext, Object2.GUID, MetadataSchema.GUID, Afrikaans.LanguageCode, null, "<demo><title>test</title><abstract>test</abstract><description>test</description></demo>" );
			Assert.AreEqual(1, result.Value);
		}

        [Test, ExpectedException(typeof(InvalidRevisionException))]
        public void Should_Throw_InvalidRevisionException_If_RevisionID_Is_NULL_When_Metadata_Already_Exist()
		{
			MCMModule.Metadata_Set( AdminCallContext, Object1.GUID, MetadataSchema.GUID, Afrikaans.LanguageCode, null, "<demo><title>test</title><abstract>test</abstract><description>test</description></demo>" );
		}

        [Test, ExpectedException(typeof(InvalidRevisionException))]
        public void Should_Throw_InvalidRevisionException_If_RevisionID_Is_Outdated()
		{
			Assert.Ignore();
         //   MCMModule.Metadata_Set(AdminCallContext, Object1.GUID, MetadataSchema.GUID, Afrikaans.LanguageCode, 1, "<demo><title>test</title><abstract>test</abstract><description>test</description></demo>");
         //   MCMModule.Metadata_Set(AdminCallContext, Object1.GUID, MetadataSchema.GUID, Afrikaans.LanguageCode, 1, "<demo><title>test</title><abstract>test</abstract><description>test</description></demo>");
		}

		//[Test]
		//public void Should_Get_Metadata()
		//{
		//    IEnumerable<Metadata> metadatas = MCMModule.Metadata_Get( AdminCallContext, Object1.GUID.ToString(), null, null );

		//    Assert.Greater(metadatas.Count(),0);
		//}
    }
}
