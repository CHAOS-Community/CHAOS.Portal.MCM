using System.Linq;
using System.Xml.Linq;
using CHAOS.MCM.Data.DTO;
using CHAOS.MCM.Module;
using NUnit.Framework;

namespace CHAOS.MCM.Test
{
    [TestFixture]
    public class DestinationInfoTest : MCMTestBase
    {
		[Test]
		public void Should_Get_Destination()
		{
			DestinationInfo dest = MCMModule.Destination_Get( AdminCallContext, DestinationInfo.ID ).First();

			Assert.AreEqual( DestinationInfo.ID, dest.ID );
		}
    }
}
