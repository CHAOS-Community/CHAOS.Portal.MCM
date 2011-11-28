using System.Collections.Generic;
using Geckon.Common.Extensions;
using Geckon.MCM.Data.Linq;
using NUnit.Framework;

namespace Geckon.MCM.Module.Standard.Test
{
    [TestFixture]
    public class MetadataTest : BaseTest
    {
        [Test]
        public void Should_Set_Metadata()
        {
            var result = MCMModule.Metadata_Set( AdminCallContext, Object1.GUID, MetadataSchema.ID, Afrikaans.LanguageCode, "<demo><title>test</title><abstract>test</abstract><description>test</description></demo>" ) ;

            Assert.AreEqual(1, result.Value);
        }

        [Test]
        public void Should_Get_Metadata()
        {
            IEnumerable<Metadata> metadatas = MCMModule.Metadata_Get( AdminCallContext, "0876EBF6-E30F-4A43-9B6E-F8A479F38427", null, null );

            Assert.Greater(metadatas.Count(),0);
        }
    }
}
