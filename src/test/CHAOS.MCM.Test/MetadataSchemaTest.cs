using System.Linq;
using CHAOS.MCM.Module;
using NUnit.Framework;

namespace CHAOS.MCM.Test
{
    [TestFixture]
    public class MetadataSchemaTest : MCMTestBase
    {
		[Test]
		public void Should_Get_All_MetadataSchemas()
		{
			var results = MCMModule.MetadataSchema_Get( AnonCallContext, null );

			Assert.Greater(results.Count(), 0);
		}
    }
}
