using System.Linq;
using NUnit.Framework;
using DestinationInfo = CHAOS.MCM.Data.DTO.DestinationInfo;

namespace Geckon.MCM.Module.Standard.Test
{
    [TestFixture]
    public class DestinationInfoTest : BaseTest
    {
		[Test]
		public void Should_Get_Destination()
		{
			DestinationInfo dest = MCMModule.Destination_Get( AdminCallContext, DestinationInfo.ID ).First();

			Assert.AreEqual( DestinationInfo.ID, dest.ID );
		}
    }
}
