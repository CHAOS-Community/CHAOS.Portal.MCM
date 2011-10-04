using Geckon.Portal.Core.Standard.Extension;
using Geckon.Portal.Extensions.Standard.Test;
using NUnit.Framework;

namespace Geckon.MCM.Module.Standard.Test
{
    [TestFixture]
    public class MetadataTest : BaseTest
    {
        [Test]
        public void Should_Set_Metadata()
        {
            var result = MCMModule.Metadata_Set( new CallContext( new MockCache(), new MockSolr(), AdminSession.SessionID.ToString() ), Object.GUID.ToString(), MetadataSchema.GUID.ToString(), Afrikaans.ID, "<demo><title>test</title><abstract>test</abstract><description>test</description></demo>" ) ;

            Assert.AreEqual(1, result.Value);
        }
    }
}
